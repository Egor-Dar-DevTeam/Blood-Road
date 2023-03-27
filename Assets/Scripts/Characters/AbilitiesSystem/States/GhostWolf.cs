using Characters.Animations;
using Characters.Information.Structs;
using UnityEngine;

namespace Characters.AbilitiesSystem.States
{
    public class GhostWolf : AbilityBase
    {
        public GhostWolf(){}
        public GhostWolf(IAnimationCommand animation, StateInfo stateInfo, VFXTransforms vfxTransforms) : base(animation, stateInfo, vfxTransforms)
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