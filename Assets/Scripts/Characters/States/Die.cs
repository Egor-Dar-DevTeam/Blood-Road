using System.Threading.Tasks;
using Characters.Animations;
using Characters.Information.Structs;
using UnityEngine;

namespace Characters.Player.States
{
    public class Die: BaseState
    {
        protected CharacterController characterController;
        public bool CanSkip { get; private set; }



        public Die(IAnimationCommand animation,StateInfo stateInfo, CharacterController characterController, VFXTransforms vfxTransforms): base( animation, stateInfo,vfxTransforms)
        {
            this.characterController = characterController;
            _parameterName = "death";
        }

        public override void Enter()
        {
            CanSkip = false;
            base.Enter();
            _animation.SetAnimation(_parameterName);
            characterController.enabled = false;
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
        }

        public override void Tick(float tickTime)
        {
        }

        public override void Exit()
        {
        }
    }
}