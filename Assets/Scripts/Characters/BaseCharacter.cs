using System.Collections.Generic;
using Characters.Facades;
using Characters.InteractableSystems;
using Characters.Player;
using JetBrains.Annotations;
using UI.CombatHUD;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace Characters
{
    public delegate bool GetIsAttack();

    public delegate void StartRechangeCurrentPoint(List<IInteractable> points);

    public delegate IInteractable GetCurrentPoint();

    public delegate bool HasCharacter();

    public abstract class BaseCharacter : MonoBehaviour, IInteractable
    {
        [SerializeField] private AnimatorOverrideController _animatorOverrideController;
        [SerializeField] private AnimationsCharacterData animationsCharacterData;
        [SerializeField] protected NavMeshAgent agent;
        [SerializeField] protected Animator animator;
        [SerializeField] protected Eyes eyesCharacters;
        [SerializeField] protected CharacterData characterData;
        [SerializeField] protected CapsuleCollider capsuleCollider;
        [SerializeField] private float rotationSpeed = 1f;
        protected bool _hasCharacter = true;

        protected IInteractable _currentPoint;

        protected SetCurrentPoint _setCurrentPoint;
        protected StartRechangeCurrentPoint _startRechangeCurrentPoint;
        protected GetCurrentPoint _getCurrentPoint;
        protected DieDelegate _characterPointDie;
        protected HasCharacter _hasCharacterDelegate;


        protected InteractionSystem _interactionSystem;
        protected TransitionAndStates _transitionAndStates;
        protected IInteractable GetCurrentPoint() => _currentPoint;
        public bool HasCharacter() => _hasCharacter;
        public Transform GetObject() => this.transform;
        public abstract bool IsPlayer();
        public DieDelegate GetDieCharacterDelegate() => _characterPointDie;
        public event DieDelegate GetDieEvent;


        protected virtual void Start()
        {
            _setCurrentPoint = SetCurrentPoint;
            _startRechangeCurrentPoint = StartRCP;
            _getCurrentPoint = GetCurrentPoint;

            _hasCharacterDelegate = HasCharacter;

            characterData.DieEvent += () => _hasCharacter = false;

            _characterPointDie = ClearPoint;
            GetDieEvent = characterData.DieEvent;
        }

        protected void SubscribeDeath()
        {
             characterData.DieEvent += _transitionAndStates.DieDelegate;
        }

        protected void InitializeTransition(TransitionAndStates transitionAndStates,
            [CanBeNull] GetIsAttack getIsAttack, [CanBeNull] UpdateEnergyDelegate updateEnergyDelegate=null)
        {
            _transitionAndStates = transitionAndStates;
            _transitionAndStates.Initialize(new TASData(animator, _getCurrentPoint, transform,
                agent, getIsAttack, characterData,
                animationsCharacterData, characterData.Damage, _hasCharacterDelegate,
                capsuleCollider, _animatorOverrideController, updateEnergyDelegate));
        }

        protected void InitializeInteractionSystem([CanBeNull] CameraRay cameraRay)
        {
            _interactionSystem = new InteractionSystem();
            _interactionSystem.Initialize(cameraRay, eyesCharacters, transform, _setCurrentPoint,
                _startRechangeCurrentPoint);
        }

        protected abstract void ClearPoint();


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

        protected abstract void StartRCP(List<IInteractable> points);
        protected abstract void SetCurrentPoint(IInteractable point);

        public virtual void ReceiveDamage(int value)
        {
            characterData.Damaged(value);
        }

        public abstract void SetOutline(Material outline);


        private void OnDestroy()
        {
            _hasCharacter = false;
            _transitionAndStates.Destroy();
        }
    }
}