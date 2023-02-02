using Characters.Animations;
using Characters.Information.Structs;
using Dreamteck.Splines;
using UnityEngine.AI;

namespace Characters.Player.States
{
    public class FolowSpline : BaseState
    {
        private SplineFollower _splineFollower;

        public FolowSpline(IAnimationCommand animation, StateInfo stateInfo,
            VFXTransforms vfxTransforms, SplineFollower splineFollower, NavMeshAgent agent) : base(
            animation, stateInfo, vfxTransforms)
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