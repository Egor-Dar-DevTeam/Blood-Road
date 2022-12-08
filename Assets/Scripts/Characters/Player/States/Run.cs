using Better.UnityPatterns.Runtime.StateMachine.States;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;

namespace Characters.Player.States
{
    public class Run : BaseState
    {
        private readonly IAnimation _animation;
        private readonly NavMeshAgent _agent;
        private Transform _point;
        private Vector3 _finalPosition;
        public Run(IAnimation animation, NavMeshAgent agent)
        {
            _animation = animation;
            _agent = agent;
        }
        public void SetPoint([CanBeNull] Transform point)
        {
            _point = point;
        }
        public override void Enter()
        {
            Debug.Log("Start Run");
            _animation.Run();
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
            Debug.Log("Finish Run");
        }
    }
}