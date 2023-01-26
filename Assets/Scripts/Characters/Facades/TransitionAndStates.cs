﻿using Better.UnityPatterns.Runtime.StateMachine;
using Characters.AbilitiesSystem;
using Characters.AbilitiesSystem.Declaration;
using Characters.Animations;
using Characters.Information;
using Characters.Player;
using Characters.Player.States;
using UnityEngine;
using UnityEngine.AI;

namespace Characters.Facades
{
    public abstract class TransitionAndStates
    {
        protected StatesInfo _statesInfo;
        private AbilitiesInfo _abilitiesInfo;
        private int _currentEffectID;

        private AbilityLibrary _abilityLibrary;
        private IRunAbility _runAbility;
        
        protected IAnimationCommand _animation;
        private VFXTransforms _vfxTransforms;

        protected RunToPoint runToPointState;
        protected Idle _idleState;
        protected Attack _attackState;
        protected Shield _shieldState;
        protected Die _dieState;
        protected StateMachine<BaseState> _stateMachine;
        protected event GetIsAttack GetIsAttack;
        protected event GetCurrentPoint CurrentPoint;
        private event HasCharacter _hasCharacter;
        private DieDelegate _dieDelegate;
        private bool _isDeath;

        public DieDelegate DieDelegate => _dieDelegate;

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
            _abilitiesInfo = data.AbilitiesInfo;
        }

        protected virtual void StatesInit(Animator animator, NavMeshAgent agent, AnimatorOverrideController animatorOverrideController, VFXTransforms vfxTransforms)
        {
            _animation = new AnimatorController(animator);
            _animation.CreateAnimationChanger(animatorOverrideController);
            _vfxTransforms = vfxTransforms;
            runToPointState = new RunToPoint(_animation, agent,_statesInfo.GetState("run"), vfxTransforms);
            _idleState = new Idle(_animation, _statesInfo.GetState("idle"), vfxTransforms);
            _shieldState = new Shield(_animation, agent, _statesInfo.GetState("shield"), vfxTransforms);
            _stateMachine = new StateMachine<BaseState>();
            Ability(vfxTransforms);
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

        public void SetCurrentEffectID(int id)
        {
            _currentEffectID = id;
        }

        private void Ability(VFXTransforms vfxTransforms)
        {
            _abilityLibrary = new AbilityLibrary();
            _runAbility = new Abilities(_stateMachine, _animation, _abilitiesInfo, _idleState, vfxTransforms, _abilityLibrary);
        }

        public void Destroy()
        {
            _stateMachine.ChangeState(_idleState);
        }

        protected virtual void TransitionInit(Transform transform, NavMeshAgent agent)
        {
            // var stun = _abilityLibrary.GetValue(new Stun());
            // var droneHummer = _abilityLibrary.GetValue(new DroneHammer());
            // _stateMachine.AddTransition(stun, ()=>_currentEffectID==stun.ID);
            // _stateMachine.AddTransition(droneHummer, ()=>_currentEffectID==droneHummer.ID);
        }

        protected virtual bool isRuning(Transform transform, NavMeshAgent agent)
        {
            if (CurrentPoint?.Invoke() != null)
            {
                runToPointState.SetPoint(CurrentPoint?.Invoke().GetObject());
                var position = CurrentPoint?.Invoke().GetObject().position;
                if (position != null &&
                    Vector3.Distance(transform.position, (Vector3)position) >= agent.stoppingDistance)
                {
                    return true;
                }

                return false;
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