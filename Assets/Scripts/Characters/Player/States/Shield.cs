using Better.UnityPatterns.Runtime.StateMachine.States;

namespace Characters.Player.States
{
    public class Shield : BaseState
    {
        private IAnimation _animation;
        public Shield(IAnimation animator)
        {
            _animation = animator;
        }
        public override void Enter()
        {
            _animation.Shield();
        }

        public override void Tick(float tickTime)
        {
        }

        public override void Exit()
        {
        }
    }
}