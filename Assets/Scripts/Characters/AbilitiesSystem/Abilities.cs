using System;
using System.Collections.Generic;
using Better.UnityPatterns.Runtime.StateMachine;
using Characters.AbilitiesSystem.States;
using Characters.Animations;
using Unity.VisualScripting;
using UnityEngine;

namespace Characters.AbilitiesSystem
{
    public class Abilities : IRunAbility
    {
        private StateMachine<BaseState> _stateMachine;
        private AbilitiesInfo _info;

        private Stun _stunState;
        private DroneHammer _droneHammerState;
        public Abilities(StateMachine<BaseState> stateMachine, IAnimationCommand animationCommand, AbilitiesInfo info,
            BaseState idleState, VFXTransforms transforms, AbilityLibrary abilityLibrary)
        {
            _stateMachine = stateMachine;
            _info = info;
            _stunState = new Stun(animationCommand, _info.GetState("stun"),transforms);
            _droneHammerState = new DroneHammer(animationCommand, _info.GetState("droneHammer"), transforms);
            InitializeLibrary();
            _stateMachine.AddTransition(_stunState, idleState, ()=> _stunState.CanSkip);
            _stateMachine.AddTransition(_droneHammerState, idleState, ()=> _stunState.CanSkip);
        }

        private void InitializeLibrary()
        {
            AbilityLibrary.StaticAddAbility(typeof(Stun), new Declaration.Stun());
            AbilityLibrary.StaticAddState(typeof(Declaration.Stun),_stunState);
            
            AbilityLibrary.StaticAddAbility(typeof(DroneHammer), new Declaration.DroneHammer());
            AbilityLibrary.StaticAddState(typeof(Declaration.DroneHammer), _droneHammerState);
        }
        public void Stun()
        {
            _stateMachine.ChangeState(_stunState);
        }

        public void DroneHammer()
        {
            _stateMachine.ChangeState(_droneHammerState);
        }
        public void RunAbility(IAbilityCommand command)
        {
            command.Apply(this);
        }
    }
}