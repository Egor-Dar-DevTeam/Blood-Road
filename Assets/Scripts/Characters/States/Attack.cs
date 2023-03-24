using System.Threading.Tasks;
using Characters.Animations;
using Characters.Information.Structs;
using Object = UnityEngine.Object;
using UnityEngine;

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
        protected Transform _vfxTransform;
        private VFXSpawnType _currentVFXSpawnType = VFXSpawnType.Default;
        public bool CanSkip => _canSkip;
        private int Damage => _isPlayer ? _transitionAndStatesData.CharacterData.Damage : _damage;

        public Attack(IAnimationCommand animation, StateInfo statesInfo, int damage, bool isPlayer,
            TransitionAndStatesData data, VFXTransforms vfxTransforms) : base(
            animation, statesInfo, vfxTransforms)
        {
            _damage = damage;
            _isPlayer = isPlayer;
            _parameterName = "attack";
            _transitionAndStatesData = data;
            _vfxTransform = _vfxTransforms.Center;
        }

        public void SetStateInfo(StateInfo info)
        {
            _clip = info.Clip != null ? info.Clip : _clip;
            _parameterName = _clip.name;
            _vfxEffect = info.VFXEffect != null ? info.VFXEffect : _vfxEffect;
            _vfxTransform = info.vfxTransform != null ? info.vfxTransform : _vfxTransform;
            _currentVFXSpawnType = info.SpawnType;
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
                _interactable.ReceiveDamage(Damage);

                await Task.Delay(milliseconds);
                _canSkip = true;

            } while (_setDamage);

        }

        private void InvokeDamage()
        {
            _canSkip = false;
            _animation.SetAnimation(_parameterName);
            VFXEffect vfx = null;
            if (_vfxEffect != null)
            {
                switch (_currentVFXSpawnType)
                {
                    case VFXSpawnType.UniversalBlow:
                        vfx = Object.Instantiate(_vfxEffect, _vfxTransform);
                        break;
                    default:
                        vfx = Object.Instantiate(_vfxEffect);
                        vfx.transform.position = _vfxTransforms.Center.position;
                        break;
                }
                vfx.SetLifeTime(1.5f);
            }

            _transitionAndStatesData.CharacterData.UseEnergy();
            _interactable.ReceiveDamage(Damage);
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