using Characters.Animations;
using UnityEngine;
using baseState=Better.UnityPatterns.Runtime.StateMachine.States.BaseState;
namespace Characters
{
    public abstract class BaseState: baseState
    {
        protected IRunCommand _animation;
        protected AnimationClip _clip;
        protected  string _parameterName;


        protected BaseState(IRunCommand animation, AnimationClip clip)
        {
            _animation = animation;
            _clip = clip;
        }

        public override void Enter()
        {
            _animation.AddClip(_parameterName,_clip);
        }
        private const int SECONDS = 1000;

        public static int SecondToMilliseconds(float second)
        {
            var result = Mathf.RoundToInt(second * SECONDS);
            return result;
        }
    }
}