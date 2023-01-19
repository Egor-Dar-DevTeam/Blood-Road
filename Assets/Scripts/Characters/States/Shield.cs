using System.Threading.Tasks;
using Characters.Animations;
using Characters.Information.Structs;
using UnityEngine;
using UnityEngine.AI;

namespace Characters.Player.States
{
    public class Shield : BaseState
    {
        private NavMeshAgent _agent;
        private int _currentMilliseconds;
        public int Milliseconds => _currentMilliseconds;

        public Shield(IAnimationCommand animation, NavMeshAgent agent, StateInfo stateInfo, VFXTransforms vfxTransforms): base( animation, stateInfo,vfxTransforms)
        {
            _agent = agent;
            _parameterName = "shield";
        }
        public override void Enter()
        {
            base.Enter();
            _animation.SetAnimation(_parameterName);
            Wait();
        }

        private async void Wait()
        {
            int milliseconds = SecondToMilliseconds(_animation.LengthAnimation(_parameterName) / 2);
            _currentMilliseconds = milliseconds*2;
            await Task.Delay(milliseconds);
            _currentMilliseconds = milliseconds;
            await Task.Delay(milliseconds);
        }

        public override void Tick(float tickTime)
        {
        }

        public override void Exit()
        {
        }
    }
}