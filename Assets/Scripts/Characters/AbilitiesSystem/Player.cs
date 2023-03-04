using Better.UnityPatterns.Runtime.StateMachine;
using Characters.AbilitiesSystem.States;
using Characters.Animations;
using Characters.InteractableSystems;
using Characters.Player;

namespace Characters.AbilitiesSystem
{
    public class Player : Abilities
    {
        private Stun _stunState;
        private AttackStun _attackStunState;
        private DroneHammer _droneHammerState;
        private SwordRain _swordRain;
        private InductionCoil _inductionCoilState;
        private ManaShield _manaShield;
        private UnleashingRage _unleashingRage;

        public Player(AbilityData abilityData) : base(abilityData)
        {
            IInit<Impenetrable> initImpenerable = _manaShield;
            initImpenerable.Initialize(abilityData.ImpenetrableDelegate);
        }

        protected override void CreateStates(IAnimationCommand animationCommand, VFXTransforms transforms)
        {
            _stunState = new Stun(animationCommand, _info.GetState("stun"), transforms);
            _attackStunState = new AttackStun(animationCommand, _info.GetState("attackStun"), transforms);
            _droneHammerState = new DroneHammer(animationCommand, _info.GetState("droneHammer"), transforms);
            _swordRain = new SwordRain(animationCommand, _info.GetState("swordRain"), transforms);
            _inductionCoilState = new InductionCoil(animationCommand, _info.GetState("inductionCoil"), transforms);
            _manaShield = new ManaShield(animationCommand, _info.GetState("manaShield"), transforms);
            _unleashingRage = new UnleashingRage(animationCommand, _info.GetState("UnleashingRage"), transforms);
        }

        protected override void InitializeTransitions(BaseState idleState)
        {
            _stateMachine.AddTransition(_stunState, () =>
            {
                var value = _currentEffectType == _attackStunState.GetType();
                if (value) _currentEffectType = null;
                return value;
            });
            _stateMachine.AddTransition(_unleashingRage, idleState, () => _unleashingRage.CanSkip);
            _stateMachine.AddTransition(_inductionCoilState, idleState, () => _inductionCoilState.CanSkip);
            _stateMachine.AddTransition(_stunState, idleState, () => _stunState.CanSkip);
            _stateMachine.AddTransition(_attackStunState, idleState, () => _attackStunState.CanSkip);
            _stateMachine.AddTransition(_droneHammerState, idleState, () => _droneHammerState.CanSkip);
            _stateMachine.AddTransition(_swordRain, idleState, () => _swordRain.CanSkip);
            _stateMachine.AddTransition(_manaShield, idleState, () => _manaShield.CanSkip);
        }

        public override void StunAttack()
        {
            _stateMachine.ChangeState(_attackStunState);
        }

        public override void SwordRain()
        {
            _stateMachine.ChangeState(_swordRain);
        }

        public override void ManaShield()
        {
            _stateMachine.ChangeState(_manaShield);
        }

        public override void InductionCoin()
        {
            _stateMachine.ChangeState(_inductionCoilState);
        }

        public override void DroneHammer()
        {
            _stateMachine.ChangeState(_droneHammerState);
        }

        public override void UnleashingRage()
        {
            _stateMachine.ChangeState(_unleashingRage);
        }
    }
}