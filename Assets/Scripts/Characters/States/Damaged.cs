using Characters.Animations;
using MapSystem.Structs;

namespace Characters.Player.States
{
    public class Damaged : BaseState
    {
        public Damaged() : base(null, new View(), null)
        {
        }

        public Damaged(IAnimationCommand animation, View view, VFXTransforms vfxTransforms) : base(animation,
            view, vfxTransforms)
        {
        }

        public override void Tick(float tickTime)
        {
        }

        public override void Exit()
        {
        }
    }
}