using System.Threading.Tasks;
using Characters.Animations;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

namespace Characters.Player.States
{
    public class DieEnemy : Die
    {
        public DieEnemy(IRunCommand animation, CapsuleCollider capsuleCollider, AnimationClip clip) : base(animation, capsuleCollider)
        {
            _animation.AddClip(_parameterName, clip);
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
           _capsuleCollider.transform.GetComponent<NavMeshAgent>().enabled = false;
           _capsuleCollider.transform.DOMoveY(_capsuleCollider.transform.position.y - 2, 10f);
           //.OnComplete((() => Object.Destroy(_capsuleCollider.gameObject)));
        }
    }
}