using System.Threading.Tasks;
using Characters.Animations;
using Characters.Information.Structs;
using Characters.InteractableSystems;
using Characters.Player;
using UnityEngine;

namespace Characters.AbilitiesSystem.States
{
    public class ManaShield : AbilityBase, IInit<Impenetrable>
    {
        private event Impenetrable _impenetrable;
        public ManaShield(IAnimationCommand animation, StateInfo stateInfo, VFXTransforms vfxTransforms) : base(
            animation, stateInfo, vfxTransforms)
        {
        }

        public override void Enter()
        {
            CanSkip = false;
            if (_vfxEffect == null) return;
            var effect = GameObject.Instantiate(_vfxEffect, _vfxTransforms.Center);
            effect.SetLifeTime(6f);
            OnImpenetrable();
            CanSkip = true;
        }

        private async void OnImpenetrable()
        {
            _impenetrable?.Invoke(true);
            await Task.Delay(SecondToMilliseconds(6));
            _impenetrable?.Invoke(false);
        }

        public override void Tick(float tickTime)
        {
        }

        public override void Exit()
        {
        }

        public void Initialize(Impenetrable subscriber)
        {
            _impenetrable += subscriber;
        }
    }
}