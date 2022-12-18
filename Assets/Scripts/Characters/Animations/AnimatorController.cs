using UnityEngine;

namespace Characters.Animations
{
    public class AnimatorController : IRunCommand
    {
        private Animator _animator;

        public AnimatorController(Animator animator)
        {
            _animator = animator;
        }
        public void Trigger(string parameterName)
        {
            _animator.SetTrigger(parameterName);
        }

        public void Bool(string parameterName, bool value)
        {
            _animator.SetBool(parameterName,value);
        }

        public void Float(string parameterName, float value)
        {
            _animator.SetFloat(parameterName, value);
        }

        public void RunCommand(IAnimate command)
        {
            command.Apply(this);
        }
    }
}