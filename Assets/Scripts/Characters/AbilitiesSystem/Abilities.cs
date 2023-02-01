using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        private SwordRain _swordRain;
        private InductionCoil _inductionCoilState;

        private event GetCurrentPoint _currentPoint;

        public Abilities(StateMachine<BaseState> stateMachine, IAnimationCommand animationCommand, AbilitiesInfo info,
            BaseState idleState, VFXTransforms transforms, GetCurrentPoint currentPoint)
        {
            _stateMachine = stateMachine;
            _info = info;
            _currentPoint += currentPoint;
            CreateStates(animationCommand, transforms);
            InitializeTransitions(idleState);
        }

        private void CreateStates(IAnimationCommand animationCommand, VFXTransforms transforms)
        {
            _stunState = new Stun(animationCommand, _info.GetState("stun"), transforms);
            _attackStunState = new AttackStun(animationCommand, _info.GetState("attackStun"), transforms);
            _droneHammerState = new DroneHammer(animationCommand, _info.GetState("droneHammer"), transforms);
            _swordRain = new SwordRain(animationCommand, _info.GetState("swordRain"), transforms);
            _inductionCoilState = new InductionCoil(animationCommand, _info.GetState("inductionCoil"), transforms);
        }

        private void InitializeTransitions(BaseState idleState)
        {
            _stateMachine.AddTransition(_stunState, () =>
            {
                var value = _currentEffectType == _attackStunState.GetType();
                if (value) _currentEffectType = null;
                return value;
            });
            _stateMachine.AddTransition(_inductionCoilState, idleState, ()=> _inductionCoilState.CanSkip);
            _stateMachine.AddTransition(_stunState, idleState, () => _stunState.CanSkip);
            _stateMachine.AddTransition(_attackStunState, idleState, () => _attackStunState.CanSkip);
            _stateMachine.AddTransition(_droneHammerState, idleState, () => _droneHammerState.CanSkip);
            _stateMachine.AddTransition(_swordRain, idleState, () => _swordRain.CanSkip);
        }


        public void StunAttack()
        {
            _stateMachine.ChangeState(_attackStunState);
        }

        public void SwordRain()
        {
            _swordRain.SetEnemy(_currentPoint?.Invoke().GetObject().gameObject);
            _stateMachine.ChangeState(_swordRain);
        }

        public void InductionCoin()
        {
            _stateMachine.ChangeState(_inductionCoilState);
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