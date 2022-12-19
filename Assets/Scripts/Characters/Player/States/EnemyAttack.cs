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
            Task.WaitAll(SetDamage());
        }

        private async Task SetDamage()
        {

                _interactable.ReceiveDamage(25);
                await Task.Yield();
                // await Task.Delay(SecondToMilliseconds(_animation.LengthAnimation(_parameterName)/2f));
                //  if(_animation.LengthAnimation(0)==0) return;


        }
    }
}