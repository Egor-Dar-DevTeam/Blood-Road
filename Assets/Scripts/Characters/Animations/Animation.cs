using UnityEngine;

namespace Characters
{
    public class Animation : IAnimation
    {
        private AnimationBoolState _animationBoolState;
        private readonly string _idleParameter = "isIdle";
        private readonly string _runParameter = "isRun";
        private readonly string _dieParameter = "isDie";
        private readonly string _attackParameter = "isAttack";
        private readonly string _aggressiveParameter = "isAggressive";
        private readonly string _neutralParameter = "isNeutral";
        private readonly string _takingHitParameter = "isTaking";
        private readonly string _shieldParameter = "isShield";
    

        public void Initialize(Animator animator)
        {
            _animationBoolState = new AnimationBoolState(animator);
            _animationBoolState.SetBoolAnimation(_idleParameter);
        }
        public void Idle()
        {
            _animationBoolState.SetBoolAnimation(_idleParameter);
        }
        public void Run()
        {
            _animationBoolState.SetBoolAnimation(_runParameter);
        }
        public void Attack()
        {
            _animationBoolState.SetBoolAnimation(_attackParameter);
        }
        public void Aggressive()
        {
            _animationBoolState.SetBoolAnimation(_aggressiveParameter);
        }
        public void Neutral()
        {
            _animationBoolState.SetBoolAnimation(_neutralParameter);
        }
        public void Taking()
        {
            _animationBoolState.SetTriggerAnimation(_takingHitParameter);
        }   
        public void Die()
        {      
            _animationBoolState.SetBoolAnimation(_dieParameter);
        }

        public void Shield()
        {
            _animationBoolState.SetBoolAnimation(_shieldParameter);
        }
    }
}