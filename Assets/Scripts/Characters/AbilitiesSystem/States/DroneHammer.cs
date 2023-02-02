using Characters.Animations;
using Characters.Information.Structs;
using Unity.Mathematics;
using UnityEngine;

namespace Characters.AbilitiesSystem.States
{
    public class DroneHammer : AbilityBase
    {
        public DroneHammer(IAnimationCommand animation, StateInfo stateInfo, VFXTransforms vfxTransforms) : base(
            animation, stateInfo, vfxTransforms)
        {
        }

        public override void Enter()
        {
            CanSkip = false;
            var effect = Object.Instantiate(_vfxEffect, _vfxTransforms.Center);
            effect.transform.rotation= quaternion.identity;
            effect.SetLifeTime(18);
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