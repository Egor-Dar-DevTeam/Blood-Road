using Characters.Animations;
using MapSystem.Structs;

namespace Characters.Player.States
{
    public class Idle : BaseState
    {
        public Idle(){}
        public Idle(IAnimationCommand animation, View view, VFXTransforms vfxTransforms) : base(animation, view,vfxTransforms)
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