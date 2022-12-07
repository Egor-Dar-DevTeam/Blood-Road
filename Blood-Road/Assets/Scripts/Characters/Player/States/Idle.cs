using Better.UnityPatterns.Runtime.StateMachine.States;
using UnityEngine;

namespace Characters.Player.States
{
    public class Idle : BaseState
    {
        private IAnimation _animation;
        public Idle(IAnimation animation)
        {
            _animation = animation;
        }
        public override void Enter()
        {
            Debug.Log("Start Idle");
            _animation.Idle();
        }

        public override void Tick(float tickTime)
        {
           
        }

        public override void Exit()
        {
            Debug.Log("Finish Idle");
        }
    }
}