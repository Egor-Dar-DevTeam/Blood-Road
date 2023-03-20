using Characters.Player.States;
using UnityEngine;

namespace Characters.Facades
{
    public class SupporterTransition : TransitionAndStates
    {
        public SupporterTransition(float maxDistance)
        {
            _maxDistanceValue = maxDistance;
        }
        protected readonly float _maxDistanceValue;

        protected Parameters _followPlayerParam;
        protected Parameters _followEnemyParam;

        protected struct Parameters
        {
            public float Speed;
            public float StoppingDistance;
        }

        public enum FollowMode : int
        {
            Player = 0,
            Enemy = 1
        }

        public override void Initialize(TransitionAndStatesData data)
        {
            base.Initialize(data);
            StatesInit(data.Animator, data.RunToPointData, data.AnimatorOverrideController, data.VFXTransforms);
            data.CreateAttack(new Attack(_animation, _statesInfo.GetState("attack"), data.Damage, false, data, data.VFXTransforms));
            data.CreateDie(new DieEnemy(_animation, _statesInfo.GetState("die"), data.CharacterController, data.VFXTransforms));
            _attackState = data.Attack;
            DieEnemy dieState = (DieEnemy)data.Die;
            _dieState = data.Die;
            TransitionInit(data.Transform, data.RunToPointData);

            _followPlayerParam = new Parameters { Speed = 5f, StoppingDistance = 3f };
            _followEnemyParam = new Parameters { Speed = 12f, StoppingDistance = 2.5f };
        }

        public void SetMode(FollowMode mode)
        {
            Parameters parameters;
            switch (mode)
            {
                case FollowMode.Player:
                    parameters = _followPlayerParam;
                    break;
                case FollowMode.Enemy:
                    parameters = _followEnemyParam;
                    break;
                default:
                    throw new System.InvalidOperationException();
            }
            _runToPointState.SetParams(parameters.Speed, parameters.StoppingDistance);
        }

        protected override void TransitionInit(Transform transform, RunToPointData runToPointData)
        {
            base.TransitionInit(transform, runToPointData);
            _stateMachine.AddTransition(_idleState, _runToPointState, () =>
            {
                var point = GetCurrentPoint();

                if (point == null) { return false; }

                if (point.IsPlayer())
                    return Vector3.Distance(transform.position, GetCurrentPoint().GetObject().position) >= _maxDistanceValue;

                var dieState = (DieEnemy)_dieState;
                dieState.SetPlayerTransform(GetCurrentPoint().GetObject());
                _runToPointState.SetPoint(GetCurrentPoint().GetObject());
    
                return true;
            });
            _stateMachine.AddTransition(_runToPointState, _idleState, () => GetCurrentPoint() == null || !IsRuning(transform, runToPointData) && GetCurrentPoint().IsPlayer());
            _stateMachine.AddTransition(_runToPointState, _attackState, () => !IsRuning(transform, runToPointData) && GetCurrentPoint().IsPlayer() == false);
            _stateMachine.AddTransition(_attackState, _runToPointState, () =>
            {
                return IsRuning(transform, runToPointData);
            });
            _stateMachine.AddTransition(_attackState, _idleState, (() => GetCurrentPoint() == null));
            _stateMachine.ChangeState(_idleState);
        }
    }
}

