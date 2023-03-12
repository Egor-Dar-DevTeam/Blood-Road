using UnityEngine;

namespace Characters.Animations
{
    public class AnimationChanger
    {
        private AnimatorOverrideController animatorController;
        private AnimationClipOverrides _clipOverrides;

        private static readonly string _nameClipOverrides = "original";

        public AnimationChanger( RuntimeAnimatorController animatorController)
        {
            
            this.animatorController = new AnimatorOverrideController(animatorController);
            

            _clipOverrides = new AnimationClipOverrides(this.animatorController.overridesCount);
            this.animatorController.GetOverrides(_clipOverrides);
        }

        public void SetAnimation(AnimationClip clip, Animator animator)
        {
            if(animator == null)return;
            animator.runtimeAnimatorController = animatorController;
            animator.Rebind();
            _clipOverrides[_nameClipOverrides] = clip;
            animatorController.ApplyOverrides(_clipOverrides);
        }
    }
}