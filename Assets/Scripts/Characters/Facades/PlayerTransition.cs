using Characters.Player;
using UnityEngine;
using UnityEngine.AI;

namespace Characters.Facades
{
    public class PlayerTransition : TransitionAndStates
    {

        protected override void TransitionInit(Transform transform, NavMeshAgent agent)
        {
            _stateMachine.AddTransition(_idleState, _runState, () =>
            {
                if (GetCurrentPoint() != null) _runState.SetPoint(GetCurrentPoint().GetObject());
                return GetCurrentPoint() != null;
            });
            _stateMachine.AddTransition(_runState, _idleState, () => GetCurrentPoint() == null);
            _stateMachine.AddTransition(_runState, _shieldState, () =>
                Vector3.Distance(transform.position, GetCurrentPoint().GetObject().position) <=
                agent.stoppingDistance+.1f);
            _stateMachine.AddTransition(_shieldState, _idleState, () => GetCurrentPoint() == null);
            _stateMachine.AddTransition(_shieldState, _runState, () => isRuning(transform, agent));
            _stateMachine.AddTransition(_attackState, _runState, () => isRuning(transform, agent));
            _stateMachine.AddTransition(_shieldState, _attackState, () => IsAttack());
            _stateMachine.AddTransition(_attackState, _shieldState,() => !IsAttack() );
            _stateMachine.AddTransition(_attackState, _idleState, (() => GetCurrentPoint() == null));
            _stateMachine.ChangeState(_idleState);
        }

        protected override bool isRuning(Transform transform, NavMeshAgent agent)
        {
            if (GetCurrentPoint() != null)
            {
                _runState.SetPoint(GetCurrentPoint().GetObject());
                var position = GetCurrentPoint().GetObject().position;
                if (position != null &&
                    Vector3.Distance(transform.position, (Vector3)position) >= agent.stoppingDistance+.2f)
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
    }
}