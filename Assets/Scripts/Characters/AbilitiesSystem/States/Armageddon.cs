using Characters.Animations;
using MapSystem.Structs;
using UnityEngine;

namespace Characters.AbilitiesSystem.States
{
    public class Armageddon : AbilityBase
    {
        public Armageddon(){}
        public Armageddon(IAnimationCommand animation, View view, VFXTransforms vfxTransforms) : base(animation, view, vfxTransforms)
        {
        }

        public override void Enter()
        {
            CanSkip = false;
            if (_vfxEffect == null) return;
            var effect = GameObject.Instantiate(_vfxEffect,_vfxTransforms.Down);
            effect.SetLifeTime(SecondToMilliseconds(1f));
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