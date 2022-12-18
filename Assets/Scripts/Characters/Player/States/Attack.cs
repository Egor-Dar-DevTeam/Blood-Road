using Better.UnityPatterns.Runtime.StateMachine.States;
using Characters.Animations;
using UnityEngine;

namespace Characters.Player.States
{
    public class Attack : BaseState
    {
        private IRunCommand _animation;
        private readonly string _parameterName = "combat";
        public Attack(IRunCommand animation)
        {
            _animation = animation;
        }
        
        public override void Enter()
        {
           Debug.Log("Attack start"); 
           _animation.RunCommand(new BoolAnimation(_parameterName, true));
           _animation.RunCommand(new BoolAnimation("attack", true));
        }

        public override void Tick(float tickTime)
        {
            
        }

        public override void Exit()
        {
            Debug.Log("Attack finish");
            _animation.RunCommand(new BoolAnimation(_parameterName, false));
            _animation.RunCommand(new BoolAnimation("attack", false));

        }
    }
}