using System;
using System.Collections.Generic;
using Better.UnityPatterns.Runtime.StateMachine;
using Characters.AbilitiesSystem.States;
using Characters.Animations;
using Characters.EffectSystem;
using Characters.Information.Structs;
using Characters.LibrarySystem;
using Unity.VisualScripting;
using UnityEngine;

namespace Characters.AbilitiesSystem
{
    public class Abilities : IRunAbility
    {
        private StateMachine<BaseState> _stateMachine;
        private AbilitiesInfo _info;
        private Type _currentEffectType;

        private Stun _stunState;
        private AttackStun _attackStunState;
        private DroneHammer _droneHammerState;
        public Abilities(StateMachine<BaseState> stateMachine, IAnimationCommand animationCommand, AbilitiesInfo info,
            BaseState idleState, VFXTransforms transforms)
        {
            _stateMachine = stateMachine;
            _info = info;
            _stunState = new Stun(animationCommand, _info.GetState("stun"),transforms);
            _attackStunState = new AttackStun(animationCommand, _info.GetState("attackStun"), transforms);
            _droneHammerState = new DroneHammer(animationCommand, _info.GetState("droneHammer"), transforms);
            _stateMachine.AddTransition(_stunState, ()=>
            {
                var value = _currentEffectType == _attackStunState.GetType();
                if (value) _currentEffectType = null;
                return value;
            });
            _stateMachine.AddTransition(_stunState, idleState, ()=> _stunState.CanSkip);
            _stateMachine.AddTransition(_attackStunState, idleState, ()=> _attackStunState.CanSkip);
            _stateMachine.AddTransition(_droneHammerState, idleState, ()=> _droneHammerState.CanSkip);
        }

        
        public void StunAttack()
        {
            _stateMachine.ChangeState(_attackStunState);
        }

        public void DroneHammer()
        {
            _stateMachine.ChangeState(_droneHammerState);
        }
        public void RunAbility(IAbilityCommand command)
        {
            command.Apply(this);
        }

        public void SetTypeAbility(Type type)
        {
            _currentEffectType = type;
        }
    }
}