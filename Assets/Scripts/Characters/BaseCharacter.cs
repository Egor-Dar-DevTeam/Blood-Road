using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Characters.AbilitiesSystem;
using Characters.EffectSystem;
using Characters.Facades;
using Characters.Information;
using Characters.InteractableSystems;
using Characters.Player;
using Characters.Player.States;
using Dreamteck.Splines;
using JetBrains.Annotations;
using UI;
using UnityEngine;

namespace Characters
{
    public delegate bool GetIsAttack();

    public delegate void StartRechangeCurrentPoint(List<IInteractable> points);

    public delegate IInteractable GetCurrentPoint();

    public delegate bool HasCharacter();

    [RequireComponent(typeof(StatesInfo), typeof(AbilitiesInfo))]
    public abstract class BaseCharacter : MonoBehaviour, IInteractable
    {
        [SerializeField] private AnimatorOverrideController _animatorOverrideController;
        [SerializeField] private StatesInfo statesInfo;
        [SerializeField] protected AbilitiesInfo abilitiesInfo;
        [SerializeField] private VFXTransforms vfxTransforms;
        [SerializeField] protected RunToPointData runToPointData;
        [SerializeField] protected Animator animator;
        [SerializeField] protected Eyes eyesCharacters;
        [SerializeField] protected CharacterData characterData;
        [SerializeField] private Linker linker;
        [SerializeField] private float rotationSpeed = 1f;
        [HideInInspector] [SerializeField] public Sender Sender;
        protected bool _hasCharacter = true;

        protected IInteractable _currentPoint;

        private SetCurrentPoint _setCurrentPoint;
        private StartRechangeCurrentPoint _startRechangeCurrentPoint;
        private GetCurrentPoint _getCurrentPoint;
        private DieInteractable _characterPointDie;
        private HasCharacter _hasCharacterDelegate;

        public event AttackedAbility AttackAbility;


        private InteractionSystem _interactionSystem;
        protected TransitionAndStates _transitionAndStates;
        private IInteractable GetCurrentPoint() => _currentPoint;
        public RemoveList GetRemoveList() => RemoveList;
        public bool HasCharacter() => _hasCharacter;
        public Receiver Receiver => linker.Receiver;

        public VFXTransforms VFXTransforms => vfxTransforms;

        public virtual void Finish()
        {
        }


        public Transform GetObject() => this.transform;
        public abstract bool IsPlayer();
        public DieInteractable GetDieCharacterDelegate => _characterPointDie;

        public event DieDelegate GetDieEvent;
        

        public void SetCharacterController()
        {
            runToPointData.CharacterController = gameObject.GetComponent<CharacterController>();

        }

        protected virtual void Start()
        {
            runToPointData.ThisCharacter = transform;
            _setCurrentPoint = SetCurrentPoint;
            _startRechangeCurrentPoint = StartRCP;
            _getCurrentPoint = GetCurrentPoint;

            _hasCharacterDelegate = HasCharacter;

            _characterPointDie = ClearPoint;
            characterData.SetInteractable(this);
        }

        protected virtual void RemoveList(IInteractable enemy)
        {
        }

        public void SetCharacterData(CharacterData data, UIDelegates delegates)
        {
            characterData = data.Copy();
            characterData.EventsInitialize(delegates);
            characterData.DieEvent += () => _hasCharacter = false;
            GetDieEvent = characterData.DieEvent;
        }

        protected virtual void SubscribeDeath()
        {
            characterData.DieEvent += _transitionAndStates.DieDelegate;
            characterData.DieEvent += vfxTransforms.DieDelegate + UnsubscribeDeath;
        }

        private void UnsubscribeDeath()
        {
            _hasCharacter = false;
            characterData.DieEvent = null;
        }

        protected void SubscribeDeathMethod(DieDelegate @delegate)
        {
            characterData.DieEvent += @delegate;
        }

        protected void InitializeTransition(TransitionAndStates transitionAndStates,
            [CanBeNull] GetIsAttack getIsAttack, [CanBeNull] SplineFollower splineFollower = null,
            [CanBeNull] Money money = null
        )
        {
            _transitionAndStates = transitionAndStates;
            linker.Initialize(_transitionAndStates, characterData);
            _transitionAndStates.Initialize(new TransitionAndStatesData(animator, _getCurrentPoint, transform,
                runToPointData, getIsAttack, characterData,
                statesInfo, characterData.Damage, _hasCharacterDelegate,
                _animatorOverrideController, vfxTransforms,
                splineFollower, money));
        }

        protected virtual void InitializeAbility(AbilityData abilityData)
        {
        }

        protected void InitializeInteractionSystem([CanBeNull] CameraRay cameraRay)
        {
            _interactionSystem = new InteractionSystem();
            _interactionSystem.Initialize(cameraRay, eyesCharacters, transform, _setCurrentPoint,
                _startRechangeCurrentPoint);
        }

        protected abstract void ClearPoint(IInteractable interactable);


        private void Update()
        {
            _transitionAndStates.Update();

            if (_currentPoint == null || !_hasCharacter) return;
            var turnTowardNavSteeringTarget = _currentPoint.GetObject().position;

            var direction = (turnTowardNavSteeringTarget - transform.position).normalized;
            var lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }

        protected abstract void StartRCP(List<IInteractable> points);

        protected virtual void SetCurrentPoint(IInteractable point)
        {
            _transitionAndStates.SetPoint(point);
        }

        public virtual void ReceiveDamage(int value)
        {
            characterData.Damaged(value);
            _transitionAndStates.Damaged();
        }

        public abstract void SetOutline(bool value);
        

        protected void WeaponAttack()
        {
            // AttackWeapon?.Invoke(_currentPoint.Receiver, characterData.);
        }
        
        public virtual void UseAbility(IAbilityCommand abilityCommand, int value)
        {
            _transitionAndStates.RunAbility.RunAbility(abilityCommand);
            if (_currentPoint != null) AttackAbility?.Invoke(_currentPoint.Receiver, abilityCommand);
        }

        private void OnDestroy()
        {
            _transitionAndStates.Destroy();
        }
    }
}