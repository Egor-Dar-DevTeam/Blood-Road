using Characters.Animations;
using Characters.EffectSystem;
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
        protected EffectBuilder effectEffectBuilder;


        protected BaseState(IAnimationCommand animation, StateInfo stateInfo, VFXTransforms vfxTransforms)
        {
            _animation = animation;
            _clip = stateInfo.Clip;
            _vfxEffect = stateInfo.VFXEffect;
            _vfxTransforms = vfxTransforms;
        }

        public override void Enter()
        {
            if (_clip != null)
            {
                _animation.AddClip(_parameterName, _clip);
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