using Characters.Animations;
using MapSystem.Structs;
using Unity.Mathematics;
using UnityEngine;

namespace Characters.AbilitiesSystem.States
{
    public class DroneHammer : AbilityBase
    {
        public DroneHammer(){}
        public DroneHammer(IAnimationCommand animation, View view, VFXTransforms vfxTransforms) : base(
            animation, view, vfxTransforms)
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