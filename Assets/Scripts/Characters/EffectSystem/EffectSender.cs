using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

namespace Characters.EffectSystem
{
    public class EffectBuilder: IEffectCreator
    {
        private VFXEffect _vfxEffect;

        public void Instantiate(VFXEffect vfxEffect, Vector3 position, Transform parent, int lifetime)
        {
          vfxEffect  = Object.Instantiate(vfxEffect, position, quaternion.identity, parent);
          vfxEffect.SetLifeTime(lifetime);
        }

        public void AddPassiveSkills(int value)
        {
           
        }

        public void AddNegativeSkills()
        {
            
        }

        public void Triggered()
        {
            
        }

        private void Reset()
        {
            _vfxEffect = null;
        }

        public VFXEffect GetEffect()
        {
            var result = _vfxEffect;
            Reset();
            return result;
        }
    }
}