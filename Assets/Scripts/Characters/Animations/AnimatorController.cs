using System.Collections.Generic;
using UnityEngine;

namespace Characters.Animations
{
    public class AnimatorController : IAnimationCommand
    {
        private Animator _animator;
        private AnimationChanger _animationChanger;
        private Dictionary<string, AnimationClip> _clips;

        public AnimatorController(Animator animator)
        {
            _animator = animator;
            _clips = new Dictionary<string, AnimationClip>();
        }
        

        public void AddClip(string key, AnimationClip value)
        {
            if (_clips == null) _clips = new Dictionary<string, AnimationClip>();
            if (!_clips.ContainsKey(key) || !_clips.ContainsValue(value))
            {
                value.events = null;
                _clips.Add(key, value);
            }
        }

        public float LengthAnimation(string nameClip)
        {
            var length = _clips[nameClip].length;
            return length;
        }

        public void CreateAnimationChanger(AnimatorOverrideController controller)
        {
            _animationChanger = new AnimationChanger(controller);
        }

        public void SetAnimation(string nameClip)
        {
            _animationChanger.SetAnimation(_clips[nameClip], _animator);
        }
    }
}