using System.Threading;
using System.Threading.Tasks;
using Characters.Animations;
using Characters.Information.Structs;

namespace Characters.AbilitiesSystem.States
{
    public class AttackStun : AbilityBase
    {
        public AttackStun(IAnimationCommand animation, StateInfo stateInfo, VFXTransforms vfxTransforms) : base(animation, stateInfo, vfxTransforms)
        {
            _parameterName = "attackStun";
        }
        public override void Enter()
        {
            CanSkip = false;
            base.Enter();
            _animation.SetAnimation(_parameterName);
            Wait();
        }

        private async void Wait()
        {
            int milliseconds = SecondToMilliseconds(_animation.LengthAnimation(_parameterName));
            await Task.Delay(milliseconds);
            CanSkip = true;
        }

        public override void Tick(float tickTime)
        {
            
        }

        public override void Exit()
        {
        }
    }
}