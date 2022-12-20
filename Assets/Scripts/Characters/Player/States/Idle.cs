using System.Threading.Tasks;
using Better.UnityPatterns.Runtime.StateMachine.States;
using Characters.Animations;
using UnityEditor;
using UnityEngine;

namespace Characters.Player.States
{
    public class Idle : BaseState
    {
        private IRunCommand _animation;

        private readonly string _parameterName = "idle";
        private readonly string _floatParameterName = "velocity";
        public Idle(IRunCommand animation)
        {
            _animation = animation;

        }
        public override void Enter()
        {
            Debug.Log("Start Idle");
            _animation.RunCommand(new SetFloatAnimation(_floatParameterName,0));
            _animation.RunCommand(new BoolAnimation(_parameterName, true));
        }

        public override void Tick(float tickTime)
        {
           
        }

        public override void Exit()
        {
            Debug.Log("Finish Idle");
            _animation.RunCommand(new BoolAnimation(_parameterName, false));
        }
    }
}