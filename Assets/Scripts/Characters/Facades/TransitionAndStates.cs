using Better.UnityPatterns.Runtime.StateMachine;
using Better.UnityPatterns.Runtime.StateMachine.States;
using Characters.Animations;
using Characters.Player;
using Characters.Player.States;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;

namespace Characters.Facades
{
    public abstract class TransitionAndStates
    {
        private IRunCommand _animation;
        
        protected Run _runState;
        protected Idle _idleState;
        protected Attack _attackState;
        protected Shield _shieldState;
        protected StateMachine<BaseState> _stateMachine;
        protected PlayerData _playerData;
        protected event GetIsAttack GetIsAttack;
        protected event GetCurrentPoint CurrentPoint;

        public void Initialize(Animator animator, GetCurrentPoint currentPointDelegate, Transform transform,
            NavMeshAgent agent, [CanBeNull] GetIsAttack isAttackDelegate, [CanBeNull] PlayerData playerData)
        {
            _playerData = playerData;
            if (isAttackDelegate != null)
            {
                GetIsAttack = isAttackDelegate;
                isAttackDelegate += GetIsAttack;
            }

            CurrentPoint = currentPointDelegate;
            currentPointDelegate += CurrentPoint;


            _animation = new AnimatorController(animator);

            _runState = new Run(_animation, agent);
            _idleState = new Idle(_animation);
            _attackState = new Attack(_animation);
            _shieldState = new Shield(_animation,agent);
            _stateMachine = new StateMachine<BaseState>();

            TransitionInit(transform, agent);
        }

        protected abstract void TransitionInit(Transform transform, NavMeshAgent agent);

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
    }
}