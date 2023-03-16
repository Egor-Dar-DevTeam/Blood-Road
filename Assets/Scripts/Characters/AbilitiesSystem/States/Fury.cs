using System.Threading.Tasks;
using Characters.Animations;
using Characters.Information.Structs;
using Characters.Player;
using UnityEngine;

namespace Characters.AbilitiesSystem.States
{
    public class Fury: AbilityBase
    {
        private CharacterData _characterData;
        public Fury(IAnimationCommand animation, StateInfo stateInfo, VFXTransforms vfxTransforms, CharacterData characterData) : base(animation, stateInfo, vfxTransforms)
        {
            _characterData = characterData;
        }
        public override void Enter()
        {
            CanSkip = false;
            if (_vfxEffect == null) return;
            var effect = GameObject.Instantiate(_vfxEffect,_vfxTransforms.Center);
            effect.transform.rotation  = Quaternion.identity;
            effect.SetLifeTime(15f);
            Wait();
            CanSkip = true;
        }

        private async void Wait()
        {
            _characterData.SetAdditionalHealthAfterDamage(true);
            await Task.Delay(SecondToMilliseconds(15f));
            _characterData.SetAdditionalHealthAfterDamage(false);
        }

        public override void Tick(float tickTime)
        {
            
        }

        public override void Exit()
        {
            
        }
    }
}