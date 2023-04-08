using System;
using Characters.AbilitiesSystem.Declaration;
using Characters.AbilitiesSystem.States;
using Characters.Animations;
using MapSystem.Structs;

namespace Characters.AbilitiesSystem
{
    public class Enemy : Abilities
    {
        private Stun _stunState;
        private AttackStun _attackStunState;
        private int _id;

        public Enemy(AbilityData abilityData) : base(abilityData)
        {
            _id = abilityData.ID;
            CreateStates(abilityData.AnimationCommand, abilityData.VFXTransforms);
            InitializeTransitions(abilityData.IdleState);
        }

        protected override void CreateStates(IAnimationCommand animationCommand, VFXTransforms transforms)
        {
            _stateCharacterKey.SetAbilityCommand(null);
            _stateCharacterKey.SetState(typeof(Stun));
            _stateCharacterKey.SetID(_id);
            if (TryGetView(out var view))
                _stunState = new Stun(animationCommand, view, transforms);
        }

        protected override void InitializeTransitions(BaseState idleState)
        {
            _stateMachine.AddTransition(_stunState, () =>
            {
                var value = _currentEffectType == typeof(AttackStun);
                if (value) _currentEffectType = null;
                return value;
            });
            _stateMachine.AddTransition(_stunState, idleState, () => _stunState.CanSkip);
        }

        public override void SetTypeAbility(Type type)
        {
            _currentEffectType = type;
        }
    }
}