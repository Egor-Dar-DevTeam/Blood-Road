using System.Threading;
using Better.UnityPatterns.Runtime.StateMachine.States;
using Characters.Animations;
using UnityEngine;

namespace Characters.Player.States
{
    public abstract class Attack : BaseState
    {
        protected IRunCommand _animation;
        protected IInteractable _interactable;
        protected bool _setDamage;
        protected string _parameterName;
        private AnimationClip _clip;
        public Attack(IRunCommand animation, AnimationClip clip)
        {
            _animation = animation;
            _clip = clip;
        }
        public void SetPoint(IInteractable point)
        {
            _interactable = point;
        }
        
        public override void Enter()
        {
            _animation.AddClip(_parameterName,_clip);
           Debug.Log("Attack start"); 
           _animation.RunCommand(new BoolAnimation(_parameterName, true));
           _setDamage = true;
        }



        public override void Tick(float tickTime)
        {
            
        }

        public override void Exit()
        {
            Debug.Log("Attack finish");
            _setDamage = false;
            _animation.RunCommand(new BoolAnimation(_parameterName, false));
        }
        
        private const int SECONDS = 1000;
        public static int SecondToMilliseconds(float second)
        {
            var result = Mathf.RoundToInt(second * SECONDS);
            return result;
        }
    }
}