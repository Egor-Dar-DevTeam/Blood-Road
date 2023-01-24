using Characters.Player.States;
using Dreamteck.Splines;
using UnityEngine;
using UnityEngine.AI;

namespace Characters.Facades
{
    public class PlayerTransition : TransitionAndStates
    {
        private FolowSpline _folowSplineState;
        private SplineFollower _splineFollower;
        private SplinePositioner _positioner;

        public override void Initialize(TransitionAndStatesData data)
        {
            base.Initialize(data);
            _splineFollower = data.SplineFollower;
            _positioner = data.Positioner;
            StatesInit(data.Animator, data.NavMeshAgent, data.AnimatorOverrideController, data.VFXTransforms);
            data.CreateAttack(new Attack(_animation, _statesInfo.GetState("attack"), data.Damage, true, data,
                data.VFXTransforms));
            data.CreateDie(new Die(_animation, _statesInfo.GetState("die"), data.CapsuleCollider, data.VFXTransforms));
            _attackState = data.Attack;
            _dieState = data.Die;

            TransitionInit(data.Transform, data.NavMeshAgent);
        }

        protected override void StatesInit(Animator animator, NavMeshAgent agent,
            AnimatorOverrideController animatorOverrideController,
            VFXTransforms vfxTransforms)
        {
            base.StatesInit(animator, agent, animatorOverrideController, vfxTransforms);
            _folowSplineState =
                new FolowSpline(_animation, _statesInfo.GetState("run"), vfxTransforms, _splineFollower, _positioner, agent);
        }

        protected override void TransitionInit(Transform transform, NavMeshAgent agent)
        {
            //    _stateMachine.AddTransition( _damagedState, _idleState,()=>true);
            _stateMachine.AddTransition(_folowSplineState, () => GetCurrentPoint() == null);
            _stateMachine.AddTransition(_folowSplineState, runToPointState, () => isRuning(transform, agent));
            _stateMachine.AddTransition(_idleState, runToPointState, () => isRuning(transform, agent));
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
                           agent.stoppingDistance + .3f;
                });
            _stateMachine.AddTransition(runToPointState, _idleState, () => GetCurrentPoint() == null);
            _stateMachine.AddTransition(runToPointState, _shieldState, () =>
                Vector3.Distance(transform.position, GetCurrentPoint().GetObject().position) <=
                agent.stoppingDistance + .1f);
            _stateMachine.AddTransition(_shieldState, _idleState, () => GetCurrentPoint() == null);
            _stateMachine.AddTransition(_shieldState, runToPointState, () => isRuning(transform, agent));
            _stateMachine.AddTransition(_attackState, runToPointState,
                () => _attackState.CanSkip && isRuning(transform, agent));
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
            _stateMachine.ChangeState(_idleState);
        }

        protected override bool isRuning(Transform transform, NavMeshAgent agent)
        {
            if (GetCurrentPoint() != null)
            {
                runToPointState.SetPoint(GetCurrentPoint().GetObject());
                var position = GetCurrentPoint().GetObject().position;
                if (position != null &&
                    Vector3.Distance(transform.position, (Vector3)position) >= agent.stoppingDistance + .2f)
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