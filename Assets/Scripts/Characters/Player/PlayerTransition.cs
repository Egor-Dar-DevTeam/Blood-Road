using Characters.Information.Structs;
using Characters.Player.States;
using Dreamteck.Splines;
using UnityEngine;

namespace Characters.Facades
{
    public class PlayerTransition : TransitionAndStates
    {
        private FolowSpline _folowSplineState;
        private SplineFollower _splineFollower;

        public override void Initialize(TransitionAndStatesData data)
        {
            base.Initialize(data);
            _splineFollower = data.SplineFollower;
            StatesInit(data.Animator, data.RunToPointData, data.AnimatorOverrideController, data.VFXTransforms);
            data.CreateAttack(new Attack(_animation, new StateInfo(), data.Damage, true, data,
                data.VFXTransforms));
            data.CreateDie(new Die(_animation, _statesInfo.GetState("die"), data.CharacterController, data.VFXTransforms));
            _attackState = data.Attack;
            _dieState = data.Die;
            _setAttackSpeed = _attackState.SetAnimationSpeed;
            _attack = _attackState.SetStateInfo;

            TransitionInit(data.Transform, data.RunToPointData);
        }

        protected override void StatesInit(Animator animator, RunToPointData runToPointData,
            AnimatorOverrideController animatorOverrideController,
            VFXTransforms vfxTransforms)
        {
            base.StatesInit(animator, runToPointData, animatorOverrideController, vfxTransforms);
            _folowSplineState =
                new FolowSpline(_animation, _statesInfo.GetState("run"), vfxTransforms, _splineFollower);
        }

        protected override void TransitionInit(Transform transform, RunToPointData runToPointData)
        {
           base.TransitionInit(transform, runToPointData);
            _stateMachine.AddTransition(_folowSplineState, () => GetCurrentPoint() == null&& !IsStoped);
            _stateMachine.AddTransition(_folowSplineState, _runToPointState, () => IsRuning(transform, runToPointData));
            _stateMachine.AddTransition(_idleState, _runToPointState, () => IsRuning(transform, runToPointData));
            _stateMachine.AddTransition(_idleState, _shieldState,
                () =>
                {
                    var point = GetCurrentPoint();
                    if (point == null)
                    {
                        return false;
                    }

                    var objectPoint = point.GetObject();
                    return Vector3.Distance(transform.position, objectPoint.position) <=
                           runToPointData.StopDistance + .3f;
                });
            _stateMachine.AddTransition(_runToPointState, _idleState, () => GetCurrentPoint() == null);
            _stateMachine.AddTransition(_runToPointState, _shieldState, () =>
                Vector3.Distance(transform.position, GetCurrentPoint().GetObject().position) <=
                runToPointData.StopDistance + .1f);
            _stateMachine.AddTransition(_shieldState, _idleState, () => GetCurrentPoint() == null);
            _stateMachine.AddTransition(_shieldState, _runToPointState, () =>  IsRuning(transform, runToPointData));
            _stateMachine.AddTransition(_attackState, _runToPointState,
                () => _attackState.CanSkip && IsRuning(transform, runToPointData));
            _stateMachine.AddTransition(_shieldState, _attackState, () =>
            {
                if (IsAttack())
                {
                    _attackState.SetPoint(GetCurrentPoint());
                }

                return IsAttack();
            });
            _stateMachine.AddTransition(_attackState, _shieldState, () => _attackState.CanSkip && !IsAttack());
            _stateMachine.AddTransition(_attackState, _idleState,
                (() => _attackState.CanSkip && GetCurrentPoint() == null));
            _stateMachine.AddTransition(_idleState, ()=> IsStoped);
            _stateMachine.ChangeState(_idleState);
        }

        protected override bool IsRuning(Transform transform, RunToPointData runToPointData)
        {
            if (GetCurrentPoint() != null)
            {
                var position = GetCurrentPoint().GetObject().position;
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