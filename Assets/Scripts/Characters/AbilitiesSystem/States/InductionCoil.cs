using Characters.Animations;
using Characters.Information.Structs;
using UnityEngine;

namespace Characters.AbilitiesSystem.States
{
    public class InductionCoil : AbilityBase
    {
        public InductionCoil(){}
        public override void Enter()
        {
            CanSkip = false;
            if (_vfxEffect == null) return;
            var effect = GameObject.Instantiate(_vfxEffect, _vfxTransforms.Down);
            effect.SetLifeTime(3f);
            CanSkip = true;
        }


        public override void Tick(float tickTime)
        {
        }

        public override void Exit()
        {
        }

        public InductionCoil(IAnimationCommand animation, StateInfo stateInfo, VFXTransforms vfxTransforms) : base(
            animation, stateInfo, vfxTransforms)
        {
        }
    }
}