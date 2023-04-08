using Characters.Animations;
using MapSystem.Structs;
using UnityEngine;

namespace Characters.Player.States
{
    public class Die : BaseState
    {
        protected CharacterController characterController;
        public bool CanSkip { get; private set; }

        public Die(){}
        public Die(IAnimationCommand animation, View view, CharacterController characterController,
            VFXTransforms vfxTransforms) : base(animation, view, vfxTransforms)
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

        private void WaitAnimation()
        {
            if (_vfxEffect == null) return;
            var effect = Object.Instantiate(_vfxEffect, _vfxTransforms.transform.position, Quaternion.identity);
            effect.SetLifeTime(SecondToMilliseconds(_animation.LengthAnimation(_parameterName)));
        }

        public override void Tick(float tickTime)
        {
        }

        public override void Exit()
        {
        }
    }
}