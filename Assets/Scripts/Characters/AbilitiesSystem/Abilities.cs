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

        public Abilities(AbilityData abilityData)
        {
            _stateMachine = abilityData.StateMachine;
            _info = abilityData.AbilitiesInfo;
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

        public virtual void ManaShield()
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

        public virtual void UnleashingRage()
        {
        }

        public virtual void Armageddon()
        {
        }

        public virtual void Fury()
        {
        }

        public virtual void UniversalBlow()
        {
        }

        public virtual void GhostWolf()
        {
            
        }
    }
}