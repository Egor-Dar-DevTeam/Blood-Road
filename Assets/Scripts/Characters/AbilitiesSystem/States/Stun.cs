using System.Threading.Tasks;
using Better.UnityPatterns.Runtime.StateMachine;
using Characters.Animations;
using Characters.Information.Structs;
using Cinemachine;
using UnityEngine;

namespace Characters.AbilitiesSystem.States
{
    public class Stun : AbilityBase
    {
        private BaseState _idleState;
        private StateMachine<BaseState> _stateMachine;

        public Stun(IAnimationCommand animation, StateInfo stateInfo, VFXTransforms transform) : base(animation, stateInfo, transform)
        {
            _parameterName = "stun";
        }

        public override void Enter()
        {
            base.Enter();
            _animation.SetAnimation(_parameterName);
            Wait();
        }

        private async void Wait()
        {
            CanSkip = false;
            var effect = Object.Instantiate(_vfxEffect, _vfxTransforms.Up);
            effect.transform.position = _vfxTransforms.Up.position;
            effect.SetLifeTime(6);
            var milliseconds = SecondToMilliseconds(6);
            await Task.Delay(milliseconds);
            CanSkip = true;

        }

        public override void Tick(float tickTime)
        {
            
        }

        public override void Exit()
        {
            
        }


    }
}