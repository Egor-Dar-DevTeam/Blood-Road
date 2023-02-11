using System.Threading.Tasks;
using Characters.Animations;
using Characters.Information.Structs;
using UnityEngine;

namespace Characters.Player.States
{
    public class Die: BaseState
    {
        protected CapsuleCollider _capsuleCollider;
        private Rigidbody _rigidbody;
        public bool CanSkip { get; private set; }



        public Die(IAnimationCommand animation,StateInfo stateInfo, CapsuleCollider capsuleCollider, Rigidbody rigidbody, VFXTransforms vfxTransforms): base( animation, stateInfo,vfxTransforms)
        {
            _capsuleCollider = capsuleCollider;
            _rigidbody = rigidbody;
            _parameterName = "death";
        }

        public override void Enter()
        {
            CanSkip = false;
            _rigidbody.useGravity = false;
            _rigidbody.isKinematic = true;
            base.Enter();
            _animation.SetAnimation(_parameterName);
            _capsuleCollider.enabled = false;
            WaitAnimation();
        }

        private async void WaitAnimation()
        {
            VFXEffect effect = null;
            if (_vfxEffect != null)
            {
                effect = Object.Instantiate(_vfxEffect, _vfxTransforms.transform.position, Quaternion.identity);
            }

            await Task.Delay(SecondToMilliseconds(_animation.LengthAnimation(_parameterName)));
           if(effect!=null) Object.Destroy(effect.gameObject);
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