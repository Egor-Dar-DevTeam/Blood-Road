using System;
using Better.UnityPatterns.Runtime.StateMachine;
using Characters.Animations;

namespace Characters.AbilitiesSystem
{
    public abstract class Abilities : IRunAbility
    {
        protected StateMachine<BaseState> _stateMachine;
        protected AbilitiesInfo _info;
        protected Type _currentEffectType;

        public Abilities(StateMachine<BaseState> stateMachine, IAnimationCommand animationCommand, AbilitiesInfo info,
            BaseState idleState, VFXTransforms transforms)
        {
            _stateMachine = stateMachine;
            _info = info;
            CreateStates(animationCommand, transforms);
            InitializeTransitions(idleState);
        }

        protected abstract void CreateStates(IAnimationCommand animationCommand, VFXTransforms transforms);

        protected abstract void InitializeTransitions(BaseState idleState);

        public virtual void StunAttack()
        {
        }

        public virtual void SwordRain()
        {
        }

        public virtual void InductionCoin()
        {
        }


        public virtual void DroneHammer()
        {
        }

        public void RunAbility(IAbilityCommand command)
        {
            command.Apply(this);
        }

        public virtual void SetTypeAbility(Type type)
        {
            // _currentEffectType = type;
        }
    }
}