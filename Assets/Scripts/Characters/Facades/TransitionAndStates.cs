using System;
using Better.UnityPatterns.Runtime.StateMachine;
using Characters.AbilitiesSystem;
using Characters.Animations;
using Characters.Information;
using Characters.InteractableSystems;
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

        private IRunAbility _runAbility;
        
        protected IAnimationCommand _animation;
        private VFXTransforms _vfxTransforms;

        #region states

        protected RunToPoint _runToPointState;
        protected Idle _idleState;
        protected Player.States.Attack _attackState;
        protected Shield _shieldState;
        protected Die _dieState;
        protected StateMachine<BaseState> _stateMachine;

        #endregion

        #region events

        protected event GetIsAttack GetIsAttack;
        protected event GetCurrentPoint CurrentPoint;
        private event HasCharacter _hasCharacter;

        #endregion

        #region delegates

        private DieDelegate _dieDelegate;
        protected Attack _attack;
        protected SetAttackSpeed _setAttackSpeed;

        #endregion

        
        private bool _isDeath;
        public bool IsStoped;

        #region publicVariables

        public DieDelegate DieDelegate => _dieDelegate;

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

            _hasCharacter += data.HasCharacter;


            CurrentPoint += data.GetCurrentPoint;
            _statesInfo = data.StatesInfo;
        }

        protected virtual void StatesInit(Animator animator, RunToPointData runToPointData, AnimatorOverrideController animatorOverrideController,
            VFXTransforms vfxTransforms)
        {
            _animation = new AnimatorController(animator);
            _animation.CreateAnimationChanger(animatorOverrideController);
            _vfxTransforms = vfxTransforms;
            _runToPointState = new RunToPoint(_animation, runToPointData,_statesInfo.GetState("run"), vfxTransforms);
            _idleState = new Idle(_animation, _statesInfo.GetState("idle"), vfxTransforms);
            _shieldState = new Shield(_animation, _statesInfo.GetState("shield"), vfxTransforms);
            _stateMachine = new StateMachine<BaseState>();
        }

        public void SetPoint(IInteractable point)
        {
            _runToPointState.SetPoint(point.GetObject());
            _attackState.SetPoint(point);
        }

        public void Damaged()
        {
            var vfxEffect = _statesInfo.GetState("damaged").VFXEffect;
            
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
                if (position != null &&
                    Vector3.Distance(transform.position, (Vector3)position) >= runToPointData.StopDistance)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            else
            {
                return false;
            }
        }

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
            _isDeath = true;
            _stateMachine.ChangeState(_dieState);
        }
    }
}