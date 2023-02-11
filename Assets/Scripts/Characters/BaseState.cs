using Characters.Animations;
using Characters.Information.Structs;
using UnityEngine;
using baseState=Better.UnityPatterns.Runtime.StateMachine.States.BaseState;
namespace Characters
{
    public abstract class BaseState: baseState
    {
        protected IAnimationCommand _animation;
        protected AnimationClip _clip;
        protected VFXEffect _vfxEffect;
        protected VFXTransforms _vfxTransforms;
        protected string _parameterName;


        protected BaseState(IAnimationCommand animation, StateInfo stateInfo, VFXTransforms vfxTransforms)
        {
            _animation = animation;
            _vfxTransforms = vfxTransforms;
            _clip = stateInfo.Clip;
            _vfxEffect = stateInfo.VFXEffect;
        }

        public override void Enter()
        {
            if (_clip != null)
            {
                _animation.AddValue(_parameterName, _clip);
            }
        }
        private const int SECONDS = 1000;

        protected static int SecondToMilliseconds(float second)
        {
            var result = Mathf.RoundToInt(second * SECONDS);
            return result;
        }
    }
}