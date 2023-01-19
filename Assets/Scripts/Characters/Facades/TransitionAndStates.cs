using Better.UnityPatterns.Runtime.StateMachine;
using Characters.AbilitiesSystem;
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

        private IRunAbility runAbility;
        
        protected IAnimationCommand _animation;

        protected Run _runState;
        protected Idle _idleState;
        protected Attack _attackState;
        protected Shield _shieldState;
        protected Die _dieState;
       protected DamagedTest _damagedState;
        protected StateMachine<BaseState> _stateMachine;
        protected event GetIsAttack GetIsAttack;
        protected event GetCurrentPoint CurrentPoint;
        private event HasCharacter _hasCharacter;
        private DieDelegate _dieDelegate;
        private bool _isDeath;
        protected bool _damaged =false;

        public DieDelegate DieDelegate => _dieDelegate;
        public IRunAbility RunAbility => runAbility;

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

        protected void StatesInit(Animator animator, NavMeshAgent agent, AnimatorOverrideController animatorOverrideController, VFXTransforms vfxTransforms)
        {
            _animation = new AnimatorController(animator);
            _animation.CreateAnimationChanger(animatorOverrideController);

            _runState = new Run(_animation, agent,_statesInfo.GetState("run"), vfxTransforms);
            _idleState = new Idle(_animation, _statesInfo.GetState("idle"), vfxTransforms);
            _shieldState = new Shield(_animation, agent, _statesInfo.GetState("shield"), vfxTransforms);
            _damagedState = new DamagedTest(_animation, _statesInfo.GetState("damaged"), vfxTransforms);
            _stateMachine = new StateMachine<BaseState>();
            Ability(vfxTransforms);
        }

        public void Damaged()
        {
            _damaged = true;
        }

        private void Ability(VFXTransforms vfxTransforms)
        {
            runAbility = new Abilities(_stateMachine, _animation, _abilitiesInfo, _idleState, vfxTransforms);
        }

        public void Destroy()
        {
            _stateMachine.ChangeState(_idleState);
        }

        protected virtual void TransitionInit(Transform transform, NavMeshAgent agent)
        {
            _stateMachine.AddTransition(_dieState, () => _isDeath);
            _stateMachine.AddTransition(_attackState, _damagedState,
                () =>
                {
                    if (!_damaged) return false;
                    _damagedState.SetMilliseconds(_attackState.Milliseconds);
                    var a = _damaged;
                    _damaged = false;
                    return a;
                });
            _stateMachine.AddTransition(_shieldState, _damagedState,(() =>
            {
                if (!_damaged) return false;
                _damagedState.SetMilliseconds(_shieldState.Milliseconds);
                var a = _damaged;
                _damaged = false;
                return a;
            }));
            _stateMachine.AddTransition(_damagedState, _idleState, () => GetCurrentPoint() == null&&_damagedState.CanSkip);
        }

        protected virtual bool isRuning(Transform transform, NavMeshAgent agent)
        {
            if (CurrentPoint?.Invoke() != null)
            {
                _runState.SetPoint(CurrentPoint?.Invoke().GetObject());
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
        }
    }
}