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

        public override void Enter()
        {
            base.Enter();
            SetDamage();
        }

        private async Task SetDamage()
        {

                _interactable.ReceiveDamage(25);
                await Task.Delay(SecondToMilliseconds(_animation.LengthAnimation(_parameterName)/2f));

        }
    }
}