using System;
using Better.UnityPatterns.Runtime.StateMachine;
using Characters.AbilitiesSystem;
using Characters.Animations;
using Characters.Player;
using Characters.Player.States;
using Characters.States;
using MapSystem;
using MapSystem.Structs;
using UnityEngine;
using Attack = Characters.Player.Attack;
using Object = UnityEngine.Object;

namespace Characters.Facades
{
    public abstract class TransitionAndStates : IAnimatableEffect
    {
        protected Placeholder _mapStates;
        protected int _idCharacter;
        protected StateCharacterKey _stateCharacterKey;
        private IRunAbility _runAbility;
        protected Vector3 _origin;

        protected IAnimationCommand _animation;
        private VFXTransforms _vfxTransforms;

        #region states

        protected RunToPoint _runToPointState;
        protected Idle _idleState;
        protected Player.States.Attack _attackState;
        protected Shield _shieldState;
        protected Die _dieState;
        protected ExplosiveRecoil _explosiveRecoilState;

        protected StateMachine<BaseState> _stateMachine;

        #endregion

        #region events

        protected event GetIsAttack GetIsAttack;
        protected event GetCurrentPoint CurrentPoint;

        #endregion

        #region delegates

        private Action _dieDelegate;
        protected Attack _attack;
        protected SetAttackSpeed _setAttackSpeed;
        protected GetRecoil _getRecoil;

        #endregion


        public bool IsStoped;

        #region publicVariables

        public Action DieDelegate => _dieDelegate;

        public Attack Attack => _attack;
        public SetAttackSpeed SetAttackSpeed => _setAttackSpeed;
        public IRunAbility RunAbility => _runAbility;

        #endregion

        public virtual void Initialize(TransitionAndStatesData data)
        {
            _dieDelegate = Death;
            if (data.GetIsAttack != null)
            {
                GetIsAttack += data.GetIsAttack;
            }

            _idCharacter = data.ID;
            _stateCharacterKey = new StateCharacterKey();
            _stateCharacterKey.SetID(_idCharacter);
            CurrentPoint += data.GetCurrentPoint;
            _mapStates = data.MapStates;
            _getRecoil = data.GetRecoil;
        }
        public void SetRecoilData(Vector3 origin, ExplosionParameters parameters)
        {
            _origin = origin;
            _explosiveRecoilState.SetOrigin(origin);
            _explosiveRecoilState.SetParameters(parameters);
        }
        protected virtual void StatesInit(Animator animator, RunToPointData runToPointData,
            AnimatorOverrideController animatorOverrideController,
            VFXTransforms vfxTransforms)
        {
            _animation = new AnimatorController(animator);
            _animation.CreateAnimationChanger(animatorOverrideController);
            _vfxTransforms = vfxTransforms;


            _stateCharacterKey.SetState(typeof(RunToPoint));
            if (TryGetView(out var view))
                _runToPointState = new RunToPoint(_animation, runToPointData, view, vfxTransforms);

            _stateCharacterKey.SetState(typeof(Idle));
            if (TryGetView(out view))
                _idleState = new Idle(_animation, view, vfxTransforms);

            _stateCharacterKey.SetID(0);
            _stateCharacterKey.SetState(typeof(Shield));
            if (TryGetView(out view))
                _shieldState = new Shield(_animation, view, vfxTransforms);
            _stateCharacterKey.SetID(_idCharacter);
            _stateMachine = new StateMachine<BaseState>();
        }

        protected bool TryGetView(out View view)
        {
            if (_mapStates.TryGetView(_stateCharacterKey, out view))
                return true;
            return false;
        }

        public void SetPoint(IInteractable point)
        {
            _runToPointState.SetPoint(point.GetObject());
        }

        public void Damage()
        {
            _stateCharacterKey.SetID(0);
            _stateCharacterKey.SetState(typeof(Damaged));
            TryGetView(out var view);
            var vfxEffect = view.Effect;
            //_stateCharacterKey.SetID(_idCharacter);

            var effect1 = Object.Instantiate(vfxEffect, _vfxTransforms.Center);
            var effect2 = Object.Instantiate(vfxEffect, _vfxTransforms.Center);
            var effect3 = Object.Instantiate(vfxEffect, _vfxTransforms.Center);
            effect2.transform.rotation = Quaternion.LookRotation(Vector3.right);
            effect3.transform.rotation = Quaternion.LookRotation(Vector3.left);
            effect1.SetLifeTime(5f);
            effect2.SetLifeTime(5f);
            effect3.SetLifeTime(5f);
        }

        public void SetCurrentEffectID(Type type)
        {
            if (_runAbility != null) _runAbility.SetTypeAbility(type);
        }

        public void InitializeAbilities(Abilities abilities)
        {
            _runAbility = abilities;
        }

        public AbilityData ReturnReadyData(AbilityData abilityData)
        {
            abilityData.SetAnimationCommand(_animation);
            abilityData.SetIdleState(_idleState);
            abilityData.SetStateMachine(_stateMachine);
            return abilityData;
        }

        public void Destroy()
        {
            _stateMachine.ChangeState(_idleState);
        }

        protected virtual void TransitionInit(Transform transform, RunToPointData runToPointData)
        {
        }

        protected virtual bool IsRuning(Transform transform, RunToPointData runToPointData)
        {
            if (CurrentPoint?.Invoke() == null) return false;
            _runToPointState.SetPoint(CurrentPoint?.Invoke().GetObject());
            var position = CurrentPoint?.Invoke().GetObject().position;
            return position != null &&
                   Vector3.Distance(transform.position, (Vector3)position) >= runToPointData.StopDistance;

        }

        protected virtual bool IsStoodUp()
        {
            return _explosiveRecoilState.IsRecoiled;
        }

        protected bool CanRecoil() => _getRecoil.Invoke();
        public void Update()
        {
            _stateMachine.Tick(Time.deltaTime);
        }

        protected IInteractable GetInteractable()
        {
            return CurrentPoint?.Invoke();
        }

        protected bool IsAttack()
        {
            return GetIsAttack.Invoke();
        }

        private void Death()
        {
            _stateMachine.ChangeState(_dieState);
        }
    }
}