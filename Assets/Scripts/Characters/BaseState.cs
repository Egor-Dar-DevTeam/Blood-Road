using System;
using Characters.Animations;
using MapSystem.Structs;
using UnityEngine;
using baseState=Better.UnityPatterns.Runtime.StateMachine.States.BaseState;

public interface ISerializableStates{}
namespace Characters
{
    [Serializable]
    public abstract class BaseState: baseState, ISerializableStates
    {
        protected IAnimationCommand _animation;
        protected AnimationClip _clip;
        protected VFXEffect _vfxEffect;
        protected VFXTransforms _vfxTransforms;
        protected string _parameterName;

        protected BaseState(){}
        protected BaseState(IAnimationCommand animation, View view, VFXTransforms vfxTransforms)
        {
            _animation = animation;
            _vfxTransforms = vfxTransforms;
            _clip = view.Animation;
            _vfxEffect = view.Effect;
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