﻿using UnityEngine;

namespace Characters.Animations
{
    public interface IRunCommand
    {
        public void AddClip(string key, AnimationClip value);
        public float LengthAnimation(string nameClip);
        public void CreateAnimationChanger(AnimatorOverrideController controller);
        public void SetAnimation(string nameClip);
    }
}