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

        public UniversalBlow()
        {
        }

        private OverrideAttack _overrideAttack;
        protected Attack _attack;
        protected int _mileseconds;
        protected SetAttackSpeed _setAttackSpeed;
        protected Transform VfxTransform => _characterData.weaponTransforms.Center;

        public UniversalBlow(IAnimationCommand animation, StateInfo stateInfo, VFXTransforms vfxTransforms,
            CharacterData characterData, Attack attack, OverrideAttack overrideAttack
            , SetAttackSpeed setAttackSpeed) : base(animation, stateInfo, vfxTransforms)
        {
            _characterData = characterData;
            _parameterName = "UniversalBlow";
            _characterData = characterData;
            _overrideAttack = overrideAttack;
            _attack = attack;
            _setAttackSpeed = setAttackSpeed;
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
            _mileseconds = SecondToMilliseconds(_animation.LengthAnimation(_parameterName));
            await Task.Delay(_mileseconds);
            CanSkip = true;
        }

        private async void Wait()
        {
            float speed = 0.4f;
            _overrideAttack?.Invoke(new StateInfo(_vfxEffect, _clip, (int)(_mileseconds * 0.4),
                VfxTransform, VFXSpawnType.UniversalBlow, speed), true);
            _setAttackSpeed?.Invoke(speed);
            _characterData.IncreaseDamageIn(2);

            await Task.Delay(SecondToMilliseconds(10f));
            _overrideAttack?.Invoke(StateInfo.empty, false);
            _setAttackSpeed?.Invoke(1);
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