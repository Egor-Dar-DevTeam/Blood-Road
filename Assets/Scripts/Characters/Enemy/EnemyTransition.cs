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
            _stateCharacterKey.SetState(typeof(Attack));
            if (TryGetView(out var view))
                data.CreateAttack(new Attack(_animation, view, false, data, data.VFXTransforms));

            _stateCharacterKey.SetState(typeof(DieEnemy));
            if (TryGetView(out view))
                data.CreateDie(new DieEnemy(_animation, view, data.CharacterController, data.VFXTransforms));

            _attackState = data.Attack;
            DieEnemy dieState = (DieEnemy)data.Die;
            dieState.SetMoneyPrefab(data.MoneyPrefab);
            _dieState = dieState;
            _attackState.SetThisCharacter(data.CharacterData.CurrentInteractable);
            TransitionInit(data.Transform, data.RunToPointData);
        }

        protected override void StatesInit(Animator animator, RunToPointData runToPointData, AnimatorOverrideController animatorOverrideController,
            VFXTransforms vfxTransforms)
        {
            base.StatesInit(animator, runToPointData, animatorOverrideController, vfxTransforms);
            
        }

        protected override void TransitionInit(Transform transform, RunToPointData runToPointData)
        {
            base.TransitionInit(transform, runToPointData);
            _stateMachine.AddTransition(_idleState, _runToPointState, () =>
            {
                if (GetInteractable() == null) return GetInteractable() != null;
                var dieState = (DieEnemy)_dieState;
                dieState.SetPlayerTransform(GetInteractable().GetObject());
                _runToPointState.SetPoint(GetInteractable().GetObject());
                return GetInteractable() != null;
            });
            _stateMachine.AddTransition(_runToPointState, _idleState, () => GetInteractable() == null);
            _stateMachine.AddTransition(_runToPointState, _attackState, () => !IsRuning(transform, runToPointData));
            _stateMachine.AddTransition(_attackState, _runToPointState, () => IsRuning(transform, runToPointData));
            _stateMachine.AddTransition(_attackState, _idleState, (() => GetInteractable() == null));
            _stateMachine.ChangeState(_idleState);
        }
    }
}