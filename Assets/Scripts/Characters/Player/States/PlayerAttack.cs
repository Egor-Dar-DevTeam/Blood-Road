using System;
using System.Threading.Tasks;
using Characters.Animations;
using UnityEngine;

namespace Characters.Player.States
{
    public class PlayerAttack: Attack
    {
        public PlayerAttack(IRunCommand animation, AnimationClip clip) : base(animation,clip)
        {
            _parameterName = "attack";
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public override void Enter()
        {
            base.Enter();
            SetDamage();
        }

        private async void SetDamage()
        {
            var milliseconds = SecondToMilliseconds(_animation.LengthAnimation(_parameterName) / 2f);
            await Task.Delay(milliseconds);
            _interactable.ReceiveDamage(25);

        }
    }
}