using UnityEngine;

namespace Characters
{
    public class AnimationBoolState
    {
        public AnimationBoolState(Animator animator)
        {
            _currentAnimator = animator;
        }
        private Animator _currentAnimator;
        private string _currentNameBoolParameter = "";

        public void SetBoolAnimation(string newNameBoolParameter)
        {
            if (_currentNameBoolParameter != "") _currentAnimator.SetBool(_currentNameBoolParameter, false);
            _currentNameBoolParameter = newNameBoolParameter;
            _currentAnimator.SetBool(_currentNameBoolParameter, true);
        }
        public void SetTriggerAnimation(string triggerNameParameter)
        {
            _currentAnimator.SetTrigger(triggerNameParameter);
        }
    }
}