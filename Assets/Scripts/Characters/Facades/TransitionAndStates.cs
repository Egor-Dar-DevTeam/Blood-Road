using Better.UnityPatterns.Runtime.StateMachine;
using Better.UnityPatterns.Runtime.StateMachine.States;
using Characters.Player;
using Characters.Player.States;
using UnityEngine;
using UnityEngine.AI;

namespace Characters.Facades
{
    public class TransitionAndStates
    {
        private IAnimation _animation;
        private Run _runState;
        private Idle _idleState;
        private Attack _attackState;
        private Shield _shieldState;
        private StateMachine<BaseState> _stateMachine;
        private event GetIsAttack GetIsAttack;
        private event GetCurrentPoint CurrentPoint;

        public void Initialize(Animator animator, GetCurrentPoint currentPointDelegate, Transform transform,
            NavMeshAgent agent, GetIsAttack isAttackDelegate)
        {
            GetIsAttack = isAttackDelegate;
            isAttackDelegate += GetIsAttack;

            CurrentPoint = currentPointDelegate;
            currentPointDelegate += CurrentPoint;


            _animation = new Animation();
            _animation.Initialize(animator);

            _runState = new Run(_animation, agent);
            _idleState = new Idle(_animation);
            _attackState = new Attack(_animation);
            _shieldState = new Shield(_animation);
            _stateMachine = new StateMachine<BaseState>();

            TransitionInit(transform, agent);
        }

        private void TransitionInit(Transform transform, NavMeshAgent agent)
        {
            _stateMachine.AddTransition(_idleState, _runState, () =>
            {
                if (CurrentPoint?.Invoke() != null) _runState.SetPoint(CurrentPoint?.Invoke().GetObject());
                return CurrentPoint?.Invoke() != null;
            });
            _stateMachine.AddTransition(_runState, _idleState, () => CurrentPoint?.Invoke() == null);
            _stateMachine.AddTransition(_runState, _shieldState, () =>
                CurrentPoint != null &&
                Vector3.Distance(transform.position, CurrentPoint.Invoke().GetObject().position) <=
                agent.stoppingDistance);
            _stateMachine.AddTransition(_shieldState, _idleState, () => CurrentPoint?.Invoke() == null);
            _stateMachine.AddTransition(_shieldState, _runState, () => isRuning(transform, agent));
            _stateMachine.AddTransition(_attackState, _runState, () => isRuning(transform, agent));
            _stateMachine.AddTransition(_shieldState, _attackState, () => GetIsAttack.Invoke());
            _stateMachine.AddTransition(_attackState, _idleState, (() => CurrentPoint.Invoke() == null));
            _stateMachine.ChangeState(_idleState);
        }

        private bool isRuning(Transform transform, NavMeshAgent agent)
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
    }
}