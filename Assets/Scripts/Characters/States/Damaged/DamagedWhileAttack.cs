using Characters.Animations;
using Characters.Information.Structs;
using UnityEngine;

namespace Characters.Player.States.Damaged
{
    public class DamagedWhileAttack: BaseState
    {
        public DamagedWhileAttack(IAnimationCommand animation, StateInfo stateInfo, VFXTransforms vfxTransforms) : base(animation, stateInfo, vfxTransforms)
        {
        }

        public override void Enter()
        {
            Object.Instantiate(_vfxEffect, _vfxTransforms.Center);
        }

        public override void Tick(float tickTime)
        {
        }

        public override void Exit()
        {
            
        }
    }
}