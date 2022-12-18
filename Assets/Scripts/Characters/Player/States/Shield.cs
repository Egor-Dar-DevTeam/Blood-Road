using Better.UnityPatterns.Runtime.StateMachine.States;
using Characters.Animations;
using UnityEngine;
using UnityEngine.AI;

namespace Characters.Player.States
{
    public class Shield : BaseState
    {
        private IRunCommand _animation;
        private NavMeshAgent _agent;
        private readonly string _parameterName = "shieldStrike";
        public Shield(IRunCommand animator, NavMeshAgent agent)
        {
            _animation = animator;
            _agent = agent;
        }
        public override void Enter()
        {
            _animation.RunCommand(new BoolAnimation(_parameterName, true));
        }

        public override void Tick(float tickTime)
        {
        }

        public override void Exit()
        {
            _animation.RunCommand(new BoolAnimation(_parameterName, false));
        }
    }
}