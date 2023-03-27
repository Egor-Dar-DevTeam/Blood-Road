using System.Threading.Tasks;
using Characters.Animations;
using Characters.Information.Structs;
using Characters.Player;
using UnityEngine;

namespace Characters.AbilitiesSystem.States
{
    public class UniversalBlow : AbilityBase
    {
        private CharacterData _characterData;
public UniversalBlow(){}
        public UniversalBlow(IAnimationCommand animation, StateInfo stateInfo, VFXTransforms vfxTransforms,
            CharacterData characterData) : base(animation, stateInfo, vfxTransforms)
        {
            _characterData = characterData;
            _parameterName = "UniversalBlow";
        }

        public override void Enter()
        {
            CanSkip = false;
            base.Enter();
            _animation.SetAnimation(_parameterName);
            if (_vfxEffect == null) return;
            var effect = GameObject.Instantiate(_vfxEffect, _characterData.weaponTransforms.Center);
            effect.SetLifeTime(SecondToMilliseconds(10f));
            WaitAnimation();
            Wait();
        }

        private async void WaitAnimation()
        {
            int milliseconds = SecondToMilliseconds(_animation.LengthAnimation(_parameterName));
            await Task.Delay(milliseconds);
            CanSkip = true;
        }

        private async void Wait()
        {
            _characterData.IncreaseDamageIn(2);
            await Task.Delay(SecondToMilliseconds(10f));
            _characterData.IncreaseDamageIn(1);
        }

        public override void Tick(float tickTime)
        {
        }

        public override void Exit()
        {
        }
    }
}