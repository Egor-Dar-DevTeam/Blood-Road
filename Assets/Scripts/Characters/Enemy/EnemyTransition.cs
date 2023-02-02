using Characters.Player.States;
using UnityEngine;
using UnityEngine.AI;

namespace Characters.Facades
{
    public class EnemyTransition : TransitionAndStates
    {
        public override void Initialize(TransitionAndStatesData data)
        {
            base.Initialize(data);
            StatesInit(data.Animator, data.NavMeshAgent, data.AnimatorOverrideController, data.VFXTransforms);
            data.CreateAttack(new Attack(_animation, _statesInfo.GetState("attack"), data.Damage, false, data, data.VFXTransforms));
            data.CreateDie(new DieEnemy(_animation, _statesInfo.GetState("die"), data.CapsuleCollider,data.VFXTransforms));
            _attackState = data.Attack;
            _dieState = data.Die;
            Ability(new AbilitiesSystem.Enemy(_stateMachine,_animation,data.AbilitiesInfo,_idleState,data.VFXTransforms));
            TransitionInit(data.Transform, data.NavMeshAgent);
        }

        protected override void TransitionInit(Transform transform, NavMeshAgent agent)
        {
            base.TransitionInit(transform,agent);
            _stateMachine.AddTransition(_idleState, _runToPointState, () =>
            {
                if (GetCurrentPoint() != null) _runToPointState.SetPoint(GetCurrentPoint().GetObject());
                return GetCurrentPoint() != null;
            });
            _stateMachine.AddTransition(_runToPointState, _idleState, () => GetCurrentPoint() == null);
            _stateMachine.AddTransition(_runToPointState, _attackState, () =>
            {
                return !isRuning(transform, agent);
            });
            _stateMachine.AddTransition(_attackState, _runToPointState, () => isRuning(transform, agent));
            _stateMachine.AddTransition(_attackState, _idleState, (() => GetCurrentPoint() == null));
            _stateMachine.ChangeState(_idleState);
        }
    }
}