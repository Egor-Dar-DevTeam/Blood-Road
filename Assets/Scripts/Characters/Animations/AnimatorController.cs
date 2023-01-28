using System.Collections.Generic;
using Characters.MapperSystem;
using UnityEngine;

namespace Characters.Animations
{
    public class AnimatorController : MapperBase<string,AnimationClip>,IAnimationCommand
    {
        private Animator _animator;
        private AnimationChanger _animationChanger;

        public AnimatorController(Animator animator): base()
        {
            _animator = animator;
        }

        public override void AddValue(string key, AnimationClip value)
        {
            base.AddValue(key, value);
            value.events = null;
        }

        public float LengthAnimation(string nameClip)
        {
            var length = _dictionary[nameClip].length;
            return length;
        }

        public void CreateAnimationChanger(AnimatorOverrideController controller)
        {
            _animationChanger = new AnimationChanger(controller);
        }

        public void SetAnimation(string nameClip)
        {
            _animationChanger.SetAnimation(_dictionary[nameClip], _animator);
        }
    }
}