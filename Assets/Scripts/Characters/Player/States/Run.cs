using Better.UnityPatterns.Runtime.StateMachine.States;
using Characters.Animations;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;

namespace Characters.Player.States
{
    public class Run : BaseState
    {
        private readonly IRunCommand _animation;
        private readonly NavMeshAgent _agent;
        private readonly string _parameterName = "walk";
        private readonly string _floatParameterName = "velocity";
        private Transform _point;
        private Vector3 _finalPosition;
        public Run(IRunCommand animation, NavMeshAgent agent)
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
            _animation.RunCommand(new BoolAnimation(_parameterName,true));
            _finalPosition = _point.position;
            _agent.SetDestination(_finalPosition);
        }

        public override void Tick(float tickTime)
        {
            _animation.RunCommand(new SetFloatAnimation(_floatParameterName, _agent.velocity.x+_agent.velocity.z));
            if (_finalPosition == _point.position) return;
            _finalPosition = _point.position;
            _agent.SetDestination(_finalPosition);
        }

        public override void Exit()
        {
            Debug.Log("Finish Run");
            _animation.RunCommand(new BoolAnimation(_parameterName,false));
            _animation.RunCommand(new SetFloatAnimation(_floatParameterName, 0));
        }
    }
}