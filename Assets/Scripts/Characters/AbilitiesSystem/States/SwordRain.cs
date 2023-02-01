using System.Threading.Tasks;
using Characters.Animations;
using Characters.Information.Structs;
using JetBrains.Annotations;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Object = System.Object;

namespace Characters.AbilitiesSystem.States
{
    public class SwordRain : AbilityBase
    {
        private GameObject _enemy;
        private bool _stopFollow;

        public SwordRain(IAnimationCommand animation, StateInfo stateInfo, VFXTransforms vfxTransforms) : base(
            animation, stateInfo, vfxTransforms)
        {
        }

        public void SetEnemy([CanBeNull] GameObject enemy)
        {
            if (enemy == null) return;

            _enemy = enemy;
        }

        public override void Enter()
        {
            CanSkip = false;
            if (_vfxEffect == null ) return;
            var effect = GameObject.Instantiate(_vfxEffect);
            effect.transform.position = _vfxTransforms.Down.position;
            effect.SetLifeTime(3f);
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