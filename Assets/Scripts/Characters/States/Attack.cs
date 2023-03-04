using System;
using System.Threading.Tasks;
using Characters.Animations;
using Characters.Information.Structs;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Characters.Player.States
{
    public class Attack : BaseState
    {
        private IInteractable _interactable;
        private TransitionAndStatesData _transitionAndStatesData;
        private int _damage;
        private float _animationSpeed = 1;
        private bool _setDamage;
        private bool _isPlayer;
        private bool _canSkip;
        public bool CanSkip => _canSkip;

        public Attack(IAnimationCommand animation, StateInfo statesInfo, int damage, bool isPlayer,
            TransitionAndStatesData data, VFXTransforms vfxTransforms) : base(
            animation, statesInfo, vfxTransforms)
        {
            _damage = damage;
            _isPlayer = isPlayer;
            _parameterName = "attack";
            _transitionAndStatesData = data;
        }

        public void SetStateInfo(StateInfo info)
        {
            _clip = info.Clip;
            _parameterName = _clip.name;
            _vfxEffect = info.VFXEffect;
        }

        public void SetAnimationSpeed(float value)
        {
            _animationSpeed = value;
        }

        public void SetPoint(IInteractable point)
        {
            _interactable = point;
        }

        public override void Enter()
        {
            // _animationSpeed = 1;
            _canSkip = false;
            base.Enter();
            _animation.SetAnimation(_parameterName);
            _setDamage = true;
            if (!_isPlayer) SetDamage();
            else Damage();
        }

        private async void SetDamage()
        {
            do
            {
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
                _interactable.ReceiveDamage(_damage);

                await Task.Delay(milliseconds);
            } while (_setDamage);

            _canSkip = true;
        }

        private void Damage()
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

            _transitionAndStatesData.CharacterData.UseEnergy();
            _interactable.ReceiveDamage(_damage);
            _canSkip = true;
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