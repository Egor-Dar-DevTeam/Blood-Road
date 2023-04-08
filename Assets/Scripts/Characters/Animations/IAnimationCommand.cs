using MapSystem;
using UnityEngine;

namespace Characters.Animations
{
    public interface IAnimationCommand: IMapper<string, AnimationClip>
    {
        public float LengthAnimation(string nameClip);
        public void CreateAnimationChanger(AnimatorOverrideController controller);
        public void SetAnimation(string nameClip);
        public void SetSpeedAnimation(float value);
    }
}