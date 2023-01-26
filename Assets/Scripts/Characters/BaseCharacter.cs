using System.Collections.Generic;
using Characters.AbilitiesSystem;
using Characters.AbilitiesSystem.Declaration;
using Characters.Animations;
using Characters.Facades;
using Characters.Information;
using Characters.InteractableSystems;
using Characters.Player;
using Dreamteck.Splines;
using JetBrains.Annotations;
using UI.CombatHUD;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace Characters
{
    public delegate bool GetIsAttack();

    public delegate void StartRechangeCurrentPoint(List<IInteractable> points);

    public delegate IInteractable GetCurrentPoint();

    public delegate bool HasCharacter();

    [RequireComponent(typeof(StatesInfo), typeof(AbilitiesInfo))]
    public abstract class BaseCharacter : MonoBehaviour, IInteractable, IReceiveAnimator
    {
        [SerializeField] private AnimatorOverrideController _animatorOverrideController;
        [SerializeField] private StatesInfo statesInfo;
        [SerializeField] private AbilitiesInfo abilitiesInfo;
        [SerializeField] private VFXTransforms vfxTransforms;
        [SerializeField] protected NavMeshAgent agent;
        [SerializeField] protected Animator animator;
        [SerializeField] protected Eyes eyesCharacters;
        [SerializeField] protected CharacterData characterData;
        [SerializeField] protected CapsuleCollider capsuleCollider;
        [SerializeField] private float rotationSpeed = 1f;
        private bool _hasCharacter = true;

        protected IInteractable _currentPoint;

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
        public abstract bool IsPlayer();
        public DieDelegate GetDieCharacterDelegate() => _characterPointDie;

        public event DieDelegate GetDieEvent;


        protected virtual void Start()
        {
            _setCurrentPoint = SetCurrentPoint;
            _startRechangeCurrentPoint = StartRCP;
            _getCurrentPoint = GetCurrentPoint;

            _hasCharacterDelegate = HasCharacter;
            
            _characterPointDie = ClearPoint;
        }

        public void SetCharacterData(CharacterData data)
        {
            characterData = data.Copy();
            characterData.DieEvent += () => _hasCharacter = false;
            GetDieEvent = characterData.DieEvent;
        }

        protected void SubscribeDeath()
        {
            characterData.DieEvent += _transitionAndStates.DieDelegate;
        }

        protected void InitializeTransition(TransitionAndStates transitionAndStates,
            [CanBeNull] GetIsAttack getIsAttack, [CanBeNull] UpdateEnergyDelegate updateEnergyDelegate = null,
            [CanBeNull] SplineFollower splineFollower = null, [CanBeNull] SplinePositioner positioner=null)
        {
            _transitionAndStates = transitionAndStates;
            _transitionAndStates.Initialize(new TransitionAndStatesData(animator, _getCurrentPoint, transform,
                agent, getIsAttack, characterData,
                statesInfo, characterData.Damage, _hasCharacterDelegate,
                capsuleCollider, _animatorOverrideController, vfxTransforms,
                updateEnergyDelegate, abilitiesInfo, splineFollower, positioner));
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

            if (_currentPoint == null) return;
            var turnTowardNavSteeringTarget = agent.steeringTarget;

            var direction = (turnTowardNavSteeringTarget - transform.position).normalized;
            var lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }

        protected abstract void StartRCP(List<IInteractable> points);
        protected abstract void SetCurrentPoint(IInteractable point);

        public virtual void ReceiveDamage(int value)
        {
            characterData.Damaged(value);
            _transitionAndStates.Damaged();
        }

        public abstract void SetOutline(bool value);


        private void OnDestroy()
        {
            _hasCharacter = false;
            _transitionAndStates.Destroy();
        }

        public void SetCurrentEffectID(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}