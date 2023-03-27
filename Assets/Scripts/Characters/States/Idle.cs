using Characters.Animations;
using Characters.Information.Structs;

namespace Characters.Player.States
{
    public class Idle : BaseState
    {
        public Idle(){}
        public Idle(IAnimationCommand animation, StateInfo stateInfo, VFXTransforms vfxTransforms) : base(animation, stateInfo,vfxTransforms)
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