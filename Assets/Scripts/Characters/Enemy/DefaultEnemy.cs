using System;
using System.Collections.Generic;
using System.Linq;
using Characters.Facades;
using Characters.InteractableSystems;
using Characters.Player;
using UnityEngine;
using UnityEngine.AI;

namespace Characters.Enemy
{
    public class DefaultEnemy : MonoBehaviour, IInteractable
    {
        [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private Animator animator;
        [SerializeField] private Eyes eyesCharacters;
        [SerializeField] private AnimationClip trackAttackClip;
        [SerializeField] private AnimationClip trackDieClip;
        [SerializeField] private EnemyData characterData;
        [SerializeField] private CapsuleCollider capsuleCollider;
        [SerializeField] private float rotationSpeed = 1f;
        private bool _hasCharacter = true;

        private IInteractable _currentPoint;

        private SetCurrentPoint _setCurrentPoint;
        private StartRechangeCurrentPoint _startRechangeCurrentPoint;
        private GetCurrentPoint _getCurrentPoint;
        private DieDelegate _characterPointDie;
        private HasCharacter _hasCharacterDelegate;


        private InteractionSystem _interactionSystem;
        private TransitionAndStates _transitionAndStates;
        private IInteractable GetCurrentPoint() => _currentPoint;
        public bool HasCharacter() => _hasCharacter;
        public Transform GetObject() => this.transform;
        public bool IsPlayer() => false;
        public DieDelegate GetDieCharacterDelegate() => _characterPointDie;
        public event DieDelegate GetDieEvent;


        private void Start()
        {
            
            
            _setCurrentPoint = SetCurrentPoint;
            _startRechangeCurrentPoint = StartRCP;
            _getCurrentPoint = GetCurrentPoint;

            _hasCharacterDelegate = HasCharacter;

            _transitionAndStates = new EnemyTransition();
            _transitionAndStates.Initialize(new TASData(animator, _getCurrentPoint, transform,
                agent, null, null, trackAttackClip, trackDieClip, characterData.Damage, _hasCharacterDelegate,
                capsuleCollider));

            _interactionSystem = new InteractionSystem();
            _interactionSystem.Initialize(null, eyesCharacters, transform, _setCurrentPoint,
                _startRechangeCurrentPoint);
            characterData.DieEvent += _transitionAndStates.DieDelegate;
            characterData.DieEvent += () => _hasCharacter = false;

            _characterPointDie = () =>
            {
                characterData.DieEvent -= _currentPoint.GetDieCharacterDelegate();
                _currentPoint = null;
            };
        }

        private void Update()
        {
            _transitionAndStates.Update();

            if (_currentPoint != null)
            {
                var turnTowardNavSteeringTarget = agent.steeringTarget;

                Vector3 direction = (turnTowardNavSteeringTarget - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
            }
        }

        private void StartRCP(List<IInteractable> points)
        {
            foreach (var point in points.Where(point => point.IsPlayer()))
            {
                SetCurrentPoint(point);
            }
        }

        private void SetCurrentPoint(IInteractable point)
        {
            if (_currentPoint != null || _currentPoint == point || !point.HasCharacter()) return;
            _currentPoint = point;
            characterData.DieEvent += _currentPoint.GetDieCharacterDelegate();

        }

        public void ReceiveDamage(int value)
        {
            characterData.Damaged(value);
        }

        public void SetOutline(Material outline)
        {
            skinnedMeshRenderer.material = outline;
        }


        private void OnDestroy()
        {
            _hasCharacter = false;
            _transitionAndStates.Destroy();
        }
    }
}