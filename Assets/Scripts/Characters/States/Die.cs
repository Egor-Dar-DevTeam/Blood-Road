using System.Threading.Tasks;
using Characters.Animations;
using UnityEditor.Media;
using UnityEngine;

namespace Characters.Player.States
{
    public class Die: BaseState
    {
        protected CapsuleCollider _capsuleCollider;
        public bool CanSkip { get; private set; }



        public Die(IRunCommand animation,AnimationClip clip, CapsuleCollider capsuleCollider): base( animation, clip)
        {
            _capsuleCollider = capsuleCollider;
            _parameterName = "death";
        }

        public override void Enter()
        {
            CanSkip = false;
            base.Enter();
            _animation.SetAnimation(_parameterName);
            _capsuleCollider.enabled = false;
            WaitAnimation();
        }

        private async void WaitAnimation()
        {
            await Task.Delay(SecondToMilliseconds(_animation.LengthAnimation(_parameterName)));
            CanSkip = true;
        }

        public override void Tick(float tickTime)
        {
        }

        public override void Exit()
        {
        }
    }
}