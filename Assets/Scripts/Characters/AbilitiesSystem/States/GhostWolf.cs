using Characters.Animations;
using MapSystem.Structs;
using UnityEngine;

namespace Characters.AbilitiesSystem.States
{
    public class GhostWolf : AbilityBase
    {
        public GhostWolf(){}
        public GhostWolf(IAnimationCommand animation, View view, VFXTransforms vfxTransforms) : base(animation, view, vfxTransforms)
        {
        }

        public override void Enter()
        {
            CanSkip = false;
            if (_vfxEffect == null) return;
            var effect = GameObject.Instantiate(_vfxEffect,_vfxTransforms.Down.position + (Vector3.left*2), Quaternion.identity);
            effect.SetLifeTime(23);
          //  effect.GetComponent<WolfGhost>().SetMainInteractable(_vfxTransforms.Character);
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