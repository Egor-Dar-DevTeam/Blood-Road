using System;
using System.Threading.Tasks;
using Characters.Animations;
using UnityEngine;

namespace Characters.Player.States
{
    public class EnemyAttack : Attack
    {
        public EnemyAttack(IRunCommand animation, AnimationClip clip) : base(animation, clip)
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
                // await Task.Yield();
                var milliseconds = SecondToMilliseconds(_animation.LengthAnimation(_parameterName)/2);
                await Task.Delay(milliseconds);
                _interactable.ReceiveDamage(25);
                await Task.Delay(milliseconds);
                //  if(_animation.LengthAnimation(0)==0) return;
            } while (_setDamage);

        }
    }
}