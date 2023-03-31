using System;
using Better.UnityPatterns.Runtime.StateMachine;
using Characters.AbilitiesSystem;
using Characters.Animations;
using Characters.Information;
using Characters.Information.Structs;
using Characters.Player;
using Characters.Player.States;
using UnityEngine;
using Attack = Characters.Player.Attack;
using Object = UnityEngine.Object;

namespace Characters.Facades
{
    public abstract class TransitionAndStates : IAnimatableEffect
    {
        protected StatesInfo _statesInfo;
        protected Vector3 _origin;
        private IRunAbility _runAbility;
        
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

            CurrentPoint += data.GetCurrentPoint;
            _statesInfo = data.StatesInfo;
            _getRecoil = data.GetRecoil;
        }

        protected virtual void StatesInit(Animator animator, RunToPointData runToPointData, AnimatorOverrideController animatorOverrideController,
            VFXTransforms vfxTransforms)
        {
            _animation = new AnimatorController(animator);
            _animation.CreateAnimationChanger(animatorOverrideController);
            _vfxTransforms = vfxTransforms;
            _runToPointState = new RunToPoint(_animation, runToPointData,_statesInfo.GetState(typeof(RunToPoint)), vfxTransforms);
            _idleState = new Idle(_animation, _statesInfo.GetState(typeof(Idle)), vfxTransforms);
            _shieldState = new Shield(_animation, _statesInfo.GetState(typeof(Shield)), vfxTransforms);
            _stateMachine = new StateMachine<BaseState>();
        }

        public void SetRecoilData(Vector3 origin, ExplosionParameters parameters)
        {
            _origin = origin;
            _explosiveRecoilState.SetOrigin(origin);
            _explosiveRecoilState.SetParameters(parameters);
        }

        public void SetPoint(IInteractable point)
        {
            _runToPointState.SetPoint(point.GetObject());
            _attackState.SetPoint(point);
        }

        public void Damaged()
        {
            var vfxEffect = _statesInfo.GetState(typeof(Damaged)).VFXEffect;
            
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
           if(_runAbility!=null) _runAbility.SetTypeAbility(type);
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
            if (CurrentPoint?.Invoke() != null)
            {
                _runToPointState.SetPoint(CurrentPoint?.Invoke().GetObject());
                var position = CurrentPoint?.Invoke().GetObject().position;
                return position != null &&
                       Vector3.Distance(transform.position, (Vector3)position) >= runToPointData.StopDistance;

            }
            else
            {
                return false;
            }
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

        protected IInteractable GetCurrentPoint()
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