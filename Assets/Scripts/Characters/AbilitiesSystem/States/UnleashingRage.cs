using Characters.Animations;
using MapSystem.Structs;
using UnityEngine;

namespace Characters.AbilitiesSystem.States
{
    public class UnleashingRage : AbilityBase
    {
        public UnleashingRage(){}
        public UnleashingRage(IAnimationCommand animation, View view, VFXTransforms vfxTransforms) : base(animation, view, vfxTransforms)
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