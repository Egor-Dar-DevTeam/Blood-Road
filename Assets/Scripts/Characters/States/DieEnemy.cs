using System.Threading.Tasks;
using Characters.Animations;
using DG.Tweening;
using UnityEngine;

namespace Characters.Player.States
{
    public class DieEnemy : Die
    {
        public DieEnemy(IRunCommand animation,AnimationClip clip, CapsuleCollider capsuleCollider): base(animation,clip,capsuleCollider)
        {
            _animation = _animation;
        }

        public override void Enter()
        {
            base.Enter();
            DieTime();
        }
        private const int SECONDS = 1000;
        public static int SecondToMilliseconds(float second)
        {
            var result = Mathf.RoundToInt(second * SECONDS);
            return result;
        }
        private async void DieTime()
        {
            await Task.Delay(SecondToMilliseconds(_animation.LengthAnimation(_parameterName)));
           _capsuleCollider.gameObject.transform.DOMoveY(_capsuleCollider.transform.position.y - 2, 10f)
           .OnComplete((() => Object.Destroy(_capsuleCollider.gameObject)));
        }
    }
}