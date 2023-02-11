using Characters.Player.States;
using UnityEngine;

namespace Characters.Facades
{
    public class EnemyTransition : TransitionAndStates
    {
        public override void Initialize(TransitionAndStatesData data)
        {
            base.Initialize(data);
            StatesInit(data.Animator, data.RunToPointData, data.AnimatorOverrideController, data.VFXTransforms);
            data.CreateAttack(new Attack(_animation, _statesInfo.GetState("attack"), data.Damage, false, data, data.VFXTransforms));
            data.CreateDie(new DieEnemy(_animation, _statesInfo.GetState("die"), data.CapsuleCollider,data.RunToPointData.Rigidbody,data.VFXTransforms));
            _attackState = data.Attack;
            _dieState = data.Die;
            Ability(new AbilitiesSystem.Enemy(_stateMachine,_animation,data.AbilitiesInfo,_idleState,data.VFXTransforms));
            TransitionInit(data.Transform, data.RunToPointData);
        }

        protected override void TransitionInit(Transform transform, RunToPointData runToPointData)
        {
            base.TransitionInit(transform,runToPointData);
            _stateMachine.AddTransition(_idleState, _runToPointState, () =>
            {
                if (GetCurrentPoint() != null) _runToPointState.SetPoint(GetCurrentPoint().GetObject());
                return GetCurrentPoint() != null;
            });
            _stateMachine.AddTransition(_runToPointState, _idleState, () => GetCurrentPoint() == null);
            _stateMachine.AddTransition(_runToPointState, _attackState, () =>
            {
                return !IsRuning(transform, runToPointData);
            });
            _stateMachine.AddTransition(_attackState, _runToPointState, () => IsRuning(transform, runToPointData));
            _stateMachine.AddTransition(_attackState, _idleState, (() => GetCurrentPoint() == null));
            _stateMachine.ChangeState(_idleState);
        }
    }
}