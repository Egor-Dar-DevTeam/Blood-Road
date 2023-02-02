using System;
using Better.UnityPatterns.Runtime.StateMachine;
using Characters.AbilitiesSystem.States;
using Characters.Animations;

namespace Characters.AbilitiesSystem
{
    public class Player : Abilities
    {
        private Stun _stunState;
        private AttackStun _attackStunState;
        private DroneHammer _droneHammerState;
        private SwordRain _swordRain;
        private InductionCoil _inductionCoilState;

        public Player(StateMachine<BaseState> stateMachine, IAnimationCommand animationCommand, AbilitiesInfo info,
            BaseState idleState, VFXTransforms transforms) : base(stateMachine, animationCommand, info, idleState,
            transforms)
        {
        }

        protected override void CreateStates(IAnimationCommand animationCommand, VFXTransforms transforms)
        {
            _stunState = new Stun(animationCommand, _info.GetState("stun"), transforms);
            _attackStunState = new AttackStun(animationCommand, _info.GetState("attackStun"), transforms);
            _droneHammerState = new DroneHammer(animationCommand, _info.GetState("droneHammer"), transforms);
            _swordRain = new SwordRain(animationCommand, _info.GetState("swordRain"), transforms);
            _inductionCoilState = new InductionCoil(animationCommand, _info.GetState("inductionCoil"), transforms);
        }

        protected override void InitializeTransitions(BaseState idleState)
        {
            _stateMachine.AddTransition(_stunState, () =>
            {
                var value = _currentEffectType == _attackStunState.GetType();
                if (value) _currentEffectType = null;
                return value;
            });
            _stateMachine.AddTransition(_inductionCoilState, idleState, () => _inductionCoilState.CanSkip);
            _stateMachine.AddTransition(_stunState, idleState, () => _stunState.CanSkip);
            _stateMachine.AddTransition(_attackStunState, idleState, () => _attackStunState.CanSkip);
            _stateMachine.AddTransition(_droneHammerState, idleState, () => _droneHammerState.CanSkip);
            _stateMachine.AddTransition(_swordRain, idleState, () => _swordRain.CanSkip);
        }

        public override void StunAttack()
        {
            _stateMachine.ChangeState(_attackStunState);
        }

        public override void SwordRain()
        {
            _stateMachine.ChangeState(_swordRain);
        }

        public override void InductionCoin()
        {
            _stateMachine.ChangeState(_inductionCoilState);
        }

        public override void DroneHammer()
        {
            _stateMachine.ChangeState(_droneHammerState);
        }
    }
}