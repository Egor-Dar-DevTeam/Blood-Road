using System.Threading.Tasks;
using Characters.Animations;
using Characters.EffectSystem;
using MapSystem;
using MapSystem.Structs;
using Object = UnityEngine.Object;
using UnityEngine;

namespace Characters.Player.States
{
    public class Attack : BaseState
    {
        private IInteractable _interactable;
        private TransitionAndStatesData _transitionAndStatesData;
        private EffectData _effectData;
        private float _animationSpeed = 1;
        private bool _setDamage;
        private bool _isPlayer;
        private bool _canSkip;

        public bool Attacked;

        protected Transform _vfxTransform;

        // private VFXSpawnType _currentVFXSpawnType = VFXSpawnType.Default;
        public bool CanSkip => _canSkip;

        public Attack()
        {
        }

        public Attack(IAnimationCommand animation, View statesInfo, bool isPlayer,
            TransitionAndStatesData data, VFXTransforms vfxTransforms) : base(
            animation, statesInfo, vfxTransforms)
        {
            _effectData = new EffectData();
            _isPlayer = isPlayer;
            _parameterName = "attack";
            _transitionAndStatesData = data;
            _vfxTransform = _vfxTransforms.Center;
        }

        public void SetStateInfo(Item item)
        {
            _clip = item.View.Animation;
            _parameterName = _clip.name;
            _vfxEffect = item.View.Effect;
            _effectData = item.Ability.EffectData;
            Attacked = true;
        }

        public void SetAnimationSpeed(float value)
        {
            _animationSpeed = value;
        }

        public void SetThisCharacter(IInteractable point)
        {
            _interactable = point;
        }

        public override void Enter()
        {
            // _animationSpeed = 1;
            base.Enter();
            _animation.SetAnimation(_parameterName);
            _setDamage = true;
            if (!_isPlayer) SetDamage();
            else InvokeDamage();
        }

        private async void SetDamage()
        {
            do
            {
                _canSkip = false;
                var milliseconds = SecondToMilliseconds(_animation.LengthAnimation(_parameterName) / 2);
                await Task.Delay(milliseconds / 2);
                VFXEffect vfx = null;
                if (_vfxEffect != null)
                {
                    vfx = Object.Instantiate(_vfxEffect);
                    vfx.transform.position = _vfxTransforms.Center.position;
                    vfx.SetLifeTime(1.5f);
                }

                await Task.Delay(milliseconds / 2);
                _transitionAndStatesData.CharacterData.UseEnergy();
                _interactable.WeaponAttack(_effectData);

                await Task.Delay(milliseconds);
                _canSkip = true;
            } while (_setDamage);
        }

        private async void InvokeDamage()
        {
            _canSkip = false;
            _animation.SetAnimation(_parameterName);
            VFXEffect vfx = null;
            if (_vfxEffect != null)
            {
                vfx = Object.Instantiate(_vfxEffect);
                vfx.transform.position = _vfxTransforms.Center.position;
                vfx.SetLifeTime(1.5f);
            }
            var milliseconds = SecondToMilliseconds(_animation.LengthAnimation(_parameterName) / 2);
            await Task.Delay(milliseconds/2);
            _transitionAndStatesData.CharacterData.UseEnergy();
            _interactable.WeaponAttack(_effectData);
            _canSkip = true;
            Attacked = false;
        }


        public override void Tick(float tickTime)
        {
            _animation.SetSpeedAnimation(_animationSpeed);
        }

        public override void Exit()
        {
            _animationSpeed = 1;
            _animation.SetSpeedAnimation(_animationSpeed);
            _setDamage = false;
        }
    }
}