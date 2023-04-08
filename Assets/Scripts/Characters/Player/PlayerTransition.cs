using Characters.Player.States;
using Dreamteck.Splines;
using UnityEngine;

namespace Characters.Facades
{
    public class PlayerTransition : TransitionAndStates
    {
        private FolowSpline _folowSplineState;
        private SplineFollower _splineFollower;
        private IInteractable _interactable;

        public override void Initialize(TransitionAndStatesData data)
        {
            base.Initialize(data);
            _interactable = data.CharacterData.CurrentInteractable;
            _splineFollower = data.SplineFollower;
            StatesInit(data.Animator, data.RunToPointData, data.AnimatorOverrideController, data.VFXTransforms);

            _stateCharacterKey.SetState(typeof(Attack));
            if (TryGetView(out var view))
                data.CreateAttack(new Attack(_animation, view, true, data,
                    data.VFXTransforms));

            _stateCharacterKey.SetState(typeof(Die));
            if (TryGetView(out view))
                data.CreateDie(new Die(_animation, view, data.CharacterController, data.VFXTransforms));

            _attackState = data.Attack;
            _dieState = data.Die;
            _setAttackSpeed = _attackState.SetAnimationSpeed;
            _attack = _attackState.SetStateInfo;
            _attackState.SetThisCharacter(_interactable);
            TransitionInit(data.Transform, data.RunToPointData);
        }

        protected override void StatesInit(Animator animator, RunToPointData runToPointData,
            AnimatorOverrideController animatorOverrideController,
            VFXTransforms vfxTransforms)
        {
            base.StatesInit(animator, runToPointData, animatorOverrideController, vfxTransforms);
            _stateCharacterKey.SetState(typeof(RunToPoint));
            if (TryGetView(out var view))
                _folowSplineState = new FolowSpline(_animation, view, vfxTransforms, _splineFollower);
        }

        protected override void TransitionInit(Transform transform, RunToPointData runToPointData)
        {
            base.TransitionInit(transform, runToPointData);
            _stateMachine.AddTransition(_folowSplineState, () => GetInteractable() == null && !IsStoped);
            _stateMachine.AddTransition(_folowSplineState, _runToPointState, () => IsRuning(transform, runToPointData));
            _stateMachine.AddTransition(_idleState, _runToPointState, () => IsRuning(transform, runToPointData));
            _stateMachine.AddTransition(_idleState, _shieldState,
                () =>
                {
                    var point = GetInteractable();
                    if (point == null)
                    {
                        return false;
                    }

                    var objectPoint = point.GetObject();
                    return Vector3.Distance(transform.position, objectPoint.position) <=
                           runToPointData.StopDistance + .3f;
                });
            _stateMachine.AddTransition(_runToPointState, _idleState, () => GetInteractable() == null);
            _stateMachine.AddTransition(_runToPointState, _shieldState, () =>
                Vector3.Distance(transform.position, GetInteractable().GetObject().position) <=
                runToPointData.StopDistance + .1f);
            _stateMachine.AddTransition(_shieldState, _idleState, () => GetInteractable() == null);
            _stateMachine.AddTransition(_shieldState, _runToPointState, () => IsRuning(transform, runToPointData));
            _stateMachine.AddTransition(_attackState, _runToPointState,
                () => _attackState.CanSkip && IsRuning(transform, runToPointData));
            _stateMachine.AddTransition(_shieldState, _attackState, () => _attackState.Attacked);
            _stateMachine.AddTransition(_attackState, _shieldState, () => _attackState.CanSkip && !_attackState.Attacked);
            _stateMachine.AddTransition(_attackState, _idleState,
                (() => _attackState.CanSkip && GetInteractable() == null));
            _stateMachine.AddTransition(_idleState, () => IsStoped);
            _stateMachine.ChangeState(_idleState);
        }

        protected override bool IsRuning(Transform transform, RunToPointData runToPointData)
        {
            if (GetInteractable() != null)
            {
                var position = GetInteractable().GetObject().position;
                if (position != null &&
                    Vector3.Distance(transform.position, (Vector3)position) >= runToPointData.StopDistance + .2f)
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