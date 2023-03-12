using System;
using Characters.AbilitiesSystem.States;
using Characters.Animations;
using Characters.Information.Structs;

namespace Characters.AbilitiesSystem
{
    public class Enemy : Abilities
    {
        private Stun _stunState;
        private AttackStun _attackStunState;

        public Enemy(AbilityData abilityData) : base(abilityData)
        {
            CreateStates(abilityData.AnimationCommand, abilityData.VFXTransforms);
            InitializeTransitions(abilityData.IdleState);
        }

        protected override void CreateStates(IAnimationCommand animationCommand, VFXTransforms transforms)
        {
            _stunState = new Stun(animationCommand, _info.GetState("stun"), transforms);
            _attackStunState = new AttackStun(null, new StateInfo(), null);
        }

        protected override void InitializeTransitions(BaseState idleState)
        {
            _stateMachine.AddTransition(_stunState, () =>
            {
                var value = _currentEffectType == _attackStunState.GetType();
                if (value) _currentEffectType = null;
                return value;
            });
            _stateMachine.AddTransition(_stunState, idleState, ()=> _stunState.CanSkip);
        }

        public override void SetTypeAbility(Type type)
        {
            _currentEffectType = type;
        }
    }
}