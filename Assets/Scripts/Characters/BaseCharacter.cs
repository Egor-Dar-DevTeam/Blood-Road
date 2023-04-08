using System.Collections.Generic;
using Characters.AbilitiesSystem;
using Characters.EffectSystem;
using Characters.Facades;
using Characters.InteractableSystems;
using Characters.Player;
using Characters.Player.States;
using Dreamteck.Splines;
using Interaction;
using JetBrains.Annotations;
using MapSystem;
using Unity.VisualScripting;
using UnityEngine;

namespace Characters
{
    public delegate bool GetIsAttack();

    public delegate void StartRechangeCurrentPoint(List<IInteractable> points);

    public delegate IInteractable GetCurrentPoint();

    public delegate bool HasCharacter();

    public abstract class BaseCharacter : MonoBehaviour, IInteractable, IInit<DieInteractable>
    {
        [SerializeField] private AnimatorOverrideController _animatorOverrideController;
        [SerializeField] private VFXTransforms vfxTransforms;
        [SerializeField] protected Placeholder mapStates;
        [SerializeField] protected RunToPointData runToPointData;
        [SerializeField] protected Animator animator;
        [SerializeField] protected Eyes eyesCharacters;
        [SerializeField] protected CharacterData characterData;
        [SerializeField] private Linker linker;
        [SerializeField] protected float rotationSpeed = 1f;
        [SerializeField] protected int iDCharacter;
        [HideInInspector] [SerializeField] public Sender Sender;
        protected bool _hasCharacter = true;

        protected IInteractable _currentPoint;

        private SetCurrentPoint _setCurrentPoint;
        private StartRechangeCurrentPoint _startRechangeCurrentPoint;
        private GetCurrentPoint getCurrentPoint;
        private DieInteractable _characterPointDie;
        private HasCharacter _hasCharacterDelegate;

        public event AttackedAbility AttackAbility;
        public event AttackedWeapon AttackWeapon;


        private InteractionSystem _interactionSystem;
        protected TransitionAndStates _transitionAndStates;
        private IInteractable GetInteractable() => _currentPoint;
        public RemoveList GetRemoveList() => RemoveList;
        public bool HasCharacter() => _hasCharacter;
        public Receiver Receiver => linker.Receiver;
        public IInit<DieInteractable> InitDie() => this;
        public ICharacterDataSubscriber CharacterDataSubscriber;

        public VFXTransforms VFXTransforms => vfxTransforms;

        public void SetPlaceholder(Placeholder placeholder) => mapStates = placeholder;

        public virtual void Finish()
        {
        }


        public Transform GetObject() => this.transform;
        public abstract bool IsPlayer();
        public DieInteractable GetDieCharacterDelegate => _characterPointDie;

        public void SetCharacterController()
        {
            runToPointData.CharacterController = GetComponent<CharacterController>();
        }

        protected virtual void Awake()
        {
            runToPointData.ThisCharacter = transform;
            _setCurrentPoint = SetCurrentPoint;
            _startRechangeCurrentPoint = StartRCP;
            getCurrentPoint = GetInteractable;

            _hasCharacterDelegate = HasCharacter;

            _characterPointDie = ClearPoint;
            characterData.SetInteractable(this);
            if (CharacterDataSubscriber == null) CharacterDataSubscriber = characterData;
        }

        protected virtual void RemoveList(IInteractable enemy)
        {
        }

        public virtual void SetCharacterData(CharacterData data)
        {
            characterData = data.Copy();
            CharacterDataSubscriber = characterData;
        }

        public void SubscribeCharacterData()
        {
            CharacterDataSubscriber = characterData;
            CharacterDataSubscriber.DieEvent += () => _hasCharacter = false;
            CharacterDataSubscriber.Damage += _transitionAndStates.Damage;
        }

        protected virtual void SubscribeDeath()
        {
            CharacterDataSubscriber.DieEvent += _transitionAndStates.DieDelegate;
            CharacterDataSubscriber.DieEvent += vfxTransforms.DieDelegate + UnsubscribeDeath;
        }

        private void UnsubscribeDeath()
        {
            _hasCharacter = false;
            CharacterDataSubscriber.DieEvent -= _transitionAndStates.DieDelegate;
        }


        protected void InitializeTransition(TransitionAndStates transitionAndStates,
            [CanBeNull] GetIsAttack getIsAttack, [CanBeNull] SplineFollower splineFollower = null,
            [CanBeNull] Money money = null
        )
        {
            _transitionAndStates = transitionAndStates;
            linker.Initialize(_transitionAndStates, characterData);
            _transitionAndStates.Initialize(new TransitionAndStatesData(animator, getCurrentPoint, transform,
                runToPointData, getIsAttack, characterData,
                mapStates, iDCharacter, _hasCharacterDelegate,
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


        protected virtual void Update()
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
            //            CharacterDataSubscriber.DieEvent += (() => _currentPoint.GetDieCharacterDelegate?.Invoke(this));
        }

        public void TakeDamage(EffectData effectData)
        {
            AttackWeapon?.Invoke(Receiver, effectData);

        }

        public abstract void SetOutline(bool value);


        public virtual void WeaponAttack(EffectData effectData)
        {
            AttackWeapon?.Invoke(_currentPoint.Receiver, effectData);
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

        public void Subscribe(DieInteractable subscriber)
        {
            characterData.DieInteractable += subscriber;
        }

        public void Unsubscribe(DieInteractable unsubscriber)
        {
            characterData.DieInteractable -= unsubscriber;
        }
    }
}