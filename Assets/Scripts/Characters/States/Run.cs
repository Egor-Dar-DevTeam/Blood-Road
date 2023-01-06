using Characters.Animations;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;

namespace Characters.Player.States
{
    public class Run : BaseState
    {
        private readonly NavMeshAgent _agent;
        private Transform _point;
        private Vector3 _finalPosition;
        public Run(IRunCommand animation, NavMeshAgent agent, AnimationClip clip): base(animation,clip)
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