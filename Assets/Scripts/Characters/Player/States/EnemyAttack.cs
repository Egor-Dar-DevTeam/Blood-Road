using System;
using System.Threading.Tasks;
using Characters.Animations;
using UnityEngine;

namespace Characters.Player.States
{
    public class EnemyAttack : Attack
    {
        public EnemyAttack(IRunCommand animation, AnimationClip clip, int damage) : base(animation, clip, damage)
        {
            _parameterName = "attack";
        }
        public override void Enter()
        {
            base.Enter();
            _animation.RunCommand(new BoolAnimation(_parameterName, true));
           SetDamage();
        }

        private async void SetDamage()
        {

            do
            {
                var milliseconds = SecondToMilliseconds(_animation.LengthAnimation(_parameterName)/2);
                await Task.Delay(milliseconds);
               _interactable.ReceiveDamage(_damage);
                await Task.Delay(milliseconds);
            } while (_setDamage);

        }
    }
}