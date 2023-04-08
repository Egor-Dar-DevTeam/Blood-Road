using Characters.Animations;
using MapSystem.Structs;

namespace Characters.Player.States
{
    public class Bottle : BaseState
    {
        public Bottle() : base(null, new View(), null)
        {
        }

        public Bottle(IAnimationCommand animation, View view, VFXTransforms vfxTransforms) : base(animation,
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