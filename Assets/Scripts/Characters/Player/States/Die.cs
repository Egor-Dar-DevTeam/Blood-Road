using Better.UnityPatterns.Runtime.StateMachine.States;
using Characters.Animations;
using UnityEngine;

namespace Characters.Player.States
{
    public class Die: BaseState
    {
        private IRunCommand _animation;
        private CapsuleCollider _capsuleCollider;

        private readonly string _parameterName = "death";

        public Die(IRunCommand animation, CapsuleCollider capsuleCollider)
        {
            _animation = animation;
            _capsuleCollider = capsuleCollider;
        }
        public override void Enter()
        {
            _animation.RunCommand(new TriggerAnimation(_parameterName));
            _capsuleCollider.enabled = false;
        }

        public override void Tick(float tickTime)
        {
        }

        public override void Exit()
        {
        }
    }
}