using Characters.Animations;
using Characters.Information.Structs;
using UnityEngine;

namespace Characters.AbilitiesSystem.States
{
    public class UnleashingRage : AbilityBase
    {
        public UnleashingRage(IAnimationCommand animation, StateInfo stateInfo, VFXTransforms vfxTransforms) : base(animation, stateInfo, vfxTransforms)
        {
        }

        public override void Enter()
        {
            CanSkip = false;
            if (_vfxEffect == null) return;
            var effect = GameObject.Instantiate(_vfxEffect,_vfxTransforms.Down.position, _vfxTransforms.Down.rotation);
            effect.SetLifeTime(SecondToMilliseconds(1.7f));
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