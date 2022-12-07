using Better.UnityPatterns.Runtime.StateMachine.States;
using UnityEngine;

namespace Characters.Player.States
{
    public class Attack : BaseState
    {
        private IAnimation _animation;
        public Attack(IAnimation animation)
        {
            _animation = animation;
        }
        
        public override void Enter()
        {
           Debug.Log("Attack start"); 
           _animation.Attack();
        }

        public override void Tick(float tickTime)
        {
            
        }

        public override void Exit()
        {
            Debug.Log("Attack finish"); 

        }
    }
}