using Characters.Player.States;
using UnityEngine;
using UnityEngine.AI;

namespace Characters.Facades
{
    public class EnemyTransition : TransitionAndStates
    {
        public override void Initialize(TASData data)
        {
            base.Initialize(data);
            StatesInit(data.Animator, data.NavMeshAgent);
            data.CreateAttack(new EnemyAttack(_animation,data.AnimationClip));
            _attackState = data.Attack;
            TransitionInit(data.Transform, data.NavMeshAgent);

        }

        protected override void TransitionInit(Transform transform, NavMeshAgent agent)
        {
            _stateMachine.AddTransition(_idleState, _runState, () =>
            {
                if (GetCurrentPoint() != null) _runState.SetPoint(GetCurrentPoint().GetObject());
                return GetCurrentPoint() != null;
            });
            _stateMachine.AddTransition(_runState, _idleState, () => GetCurrentPoint() == null);
            _stateMachine.AddTransition(_runState, _attackState, () =>
            {
                _attackState.SetPoint(GetCurrentPoint());
                return Vector3.Distance(transform.position, GetCurrentPoint().GetObject().position) <=
                       agent.stoppingDistance + .07f;
            });
            _stateMachine.AddTransition(_attackState, _runState, () => isRuning(transform, agent));
            _stateMachine.AddTransition(_attackState, _idleState, (() => GetCurrentPoint() == null));
            _stateMachine.ChangeState(_idleState);
        }
    }
}