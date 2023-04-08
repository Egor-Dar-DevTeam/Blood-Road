using Characters.Animations;
using Dreamteck.Splines;
using MapSystem.Structs;

namespace Characters.Player.States
{
    public class FolowSpline : BaseState
    {
        private SplineFollower _splineFollower;
        public FolowSpline(){}
        public FolowSpline(IAnimationCommand animation, View view,
            VFXTransforms vfxTransforms, SplineFollower splineFollower) : base(
            animation, view, vfxTransforms)
        {
            _splineFollower = splineFollower;
            _parameterName = "run";
        }

        public override void Enter()
        {
            base.Enter();
            _animation.SetAnimation(_parameterName); 
            _splineFollower.follow = true;
        }


        public override void Tick(float tickTime)
        {

        }

        public override void Exit()
        {
            _splineFollower.follow = false;
        }
    }
}