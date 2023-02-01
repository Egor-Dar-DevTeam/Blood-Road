using Characters.Animations;
using Characters.Information.Structs;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;

namespace Characters.Player.States
{
    public class RunToPoint : BaseState
    {
        private readonly NavMeshAgent _agent;
        private Transform _point;
        private Vector3 _finalPosition;
        public Transform Point => _point;

        public RunToPoint(IAnimationCommand animation, NavMeshAgent agent, StateInfo stateInfo,
            VFXTransforms vfxTransforms) : base(animation, stateInfo, vfxTransforms)
        {
            _agent = agent;
            _parameterName = "run";
        }

        public void SetPoint([CanBeNull] Transform point)
        {
            _point = point;
        }

        public override void Enter()
        {
            base.Enter();
            _animation.SetAnimation(_parameterName);
            _finalPosition = _point.position;
            _agent.SetDestination(_finalPosition);
        }

        public override void Tick(float tickTime)
        {
            if (_finalPosition == _point.position) return;
            _finalPosition = _point.position;
            _agent.SetDestination(_finalPosition);
        }

        public override void Exit()
        {
        }
    }
}