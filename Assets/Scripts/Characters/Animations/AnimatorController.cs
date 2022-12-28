using System.Collections.Generic;
using UnityEngine;

namespace Characters.Animations
{
    public class AnimatorController : IRunCommand
    {
        private Animator _animator;
        private Dictionary<string, AnimationClip> _clips;

        public AnimatorController(Animator animator)
        {
            _animator = animator;
            _clips = new Dictionary<string, AnimationClip>();
        }


        public void Trigger(string parameterName)
        {
            _animator.SetTrigger(parameterName);
        }

        public void Bool(string parameterName, bool value)
        {
            _animator.SetBool(parameterName, value);
        }

        public void Float(string parameterName, float value)
        {
            _animator.SetFloat(parameterName, value);
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

        public void RunCommand(IAnimate command)
        {
            command.Apply(this);
        }
    }
}