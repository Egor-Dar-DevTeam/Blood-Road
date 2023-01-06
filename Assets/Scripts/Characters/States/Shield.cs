using Characters.Animations;
using UnityEngine;
using UnityEngine.AI;

namespace Characters.Player.States
{
    public class Shield : BaseState
    {
        private NavMeshAgent _agent;
        public Shield(IRunCommand animation, NavMeshAgent agent, AnimationClip clip): base( animation, clip)
        {
            _agent = agent;
            _parameterName = "shield";
        }
        public override void Enter()
        {
            base.Enter();
            _animation.SetAnimation(_parameterName);
        }

        public override void Tick(float tickTime)
        {
        }

        public override void Exit()
        {
        }
    }
}