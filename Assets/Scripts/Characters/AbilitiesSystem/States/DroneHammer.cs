using System.Threading.Tasks;
using Characters.Animations;
using Characters.Information.Structs;
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
            var effect = Object.Instantiate(_vfxEffect, _vfxTransforms.Center);
            effect.SetLifeTime(18);
        }


        public override void Tick(float tickTime)
        {
        }


        public override void Exit()
        {
        }
    }
}