using Characters.Animations;
using MapSystem.Structs;
using UnityEngine;

namespace Characters.AbilitiesSystem.States
{
    public class SwordRain : AbilityBase
    {
        private GameObject _enemy;
        private bool _stopFollow;
public SwordRain(){}
        public SwordRain(IAnimationCommand animation, View view, VFXTransforms vfxTransforms) : base(
            animation, view, vfxTransforms)
        {
        }
        

        public override void Enter()
        {
            CanSkip = false;
            if (_vfxEffect == null ) return;
            var effect = GameObject.Instantiate(_vfxEffect);
            effect.transform.position = _vfxTransforms.Down.position+Vector3.forward*2;
            effect.SetLifeTime(3.5f);
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