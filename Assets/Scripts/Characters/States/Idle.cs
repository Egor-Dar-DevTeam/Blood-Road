using Characters.Animations;
using UnityEngine;

namespace Characters.Player.States
{
    public class Idle : BaseState
    {
        public Idle(IRunCommand animation, AnimationClip clip): base(animation,clip)
        {
        _parameterName = "idle";
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