using UnityEngine;

namespace Characters.Animations
{
    public interface IRunCommand
    {
        public void RunCommand(IAnimate command);
        public void AddClip(string key, AnimationClip value);
        public float LengthAnimation(string nameClip);
    }
}