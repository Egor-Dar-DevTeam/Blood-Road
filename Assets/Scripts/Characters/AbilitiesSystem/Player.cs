using Characters.AbilitiesSystem.Declaration;
using Characters.AbilitiesSystem.States;
using Characters.Animations;
using Characters.InteractableSystems;
using Characters.Player;
using Armageddon = Characters.AbilitiesSystem.States.Armageddon;
using DroneHammer = Characters.AbilitiesSystem.States.DroneHammer;
using Fury = Characters.AbilitiesSystem.States.Fury;
using GhostWolf = Characters.AbilitiesSystem.States.GhostWolf;
using InductionCoil = Characters.AbilitiesSystem.States.InductionCoil;
using ManaShield = Characters.AbilitiesSystem.States.ManaShield;
using SwordRain = Characters.AbilitiesSystem.States.SwordRain;
using UniversalBlow = Characters.AbilitiesSystem.States.UniversalBlow;
using UnleashingRage = Characters.AbilitiesSystem.States.UnleashingRage;

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
        private Armageddon _armageddon;
        private Fury _fury;
        private UniversalBlow _universalBlow;
        private GhostWolf _ghostWolf;

        public Player(AbilityData abilityData) : base(abilityData)
        {
            CreateStates(abilityData.AnimationCommand, abilityData.VFXTransforms);
            InitializeTransitions(abilityData.IdleState);
            IInit<Impenetrable> initImpenerable = _manaShield;
            initImpenerable.Subscribe(abilityData.ImpenetrableDelegate);
        }

        protected override void CreateStates(IAnimationCommand animationCommand, VFXTransforms transforms)
        {
            _stateCharacterKey.SetState(typeof(AttackStun));
            _stateCharacterKey.SetAbilityCommand(typeof(StunAttack));
            if (TryGetView(out var view))
                _attackStunState = new AttackStun(animationCommand, view, transforms);

            _stateCharacterKey.SetAbilityCommand(typeof(Declaration.DroneHammer));
            _stateCharacterKey.SetState(typeof(DroneHammer));
            if (TryGetView(out view))
                _droneHammerState = new DroneHammer(animationCommand, view, transforms);

            _stateCharacterKey.SetAbilityCommand(typeof(Declaration.SwordRain));
            _stateCharacterKey.SetState(typeof(SwordRain));
            if (TryGetView(out view))
                _swordRain = new SwordRain(animationCommand, view, transforms);

            _stateCharacterKey.SetAbilityCommand(typeof(Declaration.InductionCoil));
            _stateCharacterKey.SetState(typeof(InductionCoil));
            if (TryGetView(out view))
                _inductionCoilState = new InductionCoil(animationCommand, view, transforms);

            _stateCharacterKey.SetAbilityCommand(typeof(Declaration.ManaShield));
            _stateCharacterKey.SetState(typeof(ManaShield));
            if (TryGetView(out view))
                _manaShield = new ManaShield(animationCommand, view, transforms);

            _stateCharacterKey.SetAbilityCommand(typeof(Declaration.UnleashingRage));
            _stateCharacterKey.SetState(typeof(UnleashingRage));
            if (TryGetView(out view))
                _unleashingRage = new UnleashingRage(animationCommand, view, transforms);

            _stateCharacterKey.SetAbilityCommand(typeof(Declaration.Armageddon));
            _stateCharacterKey.SetState(typeof(Armageddon));
            if (TryGetView(out view))
                _armageddon = new Armageddon(animationCommand, view, transforms);

            _stateCharacterKey.SetAbilityCommand(typeof(Declaration.Fury));
            _stateCharacterKey.SetState(typeof(Fury));
            if (TryGetView(out view))
                _fury = new Fury(animationCommand, view, transforms);

            _stateCharacterKey.SetAbilityCommand(typeof(Declaration.UniversalBlow));
            _stateCharacterKey.SetState(typeof(UniversalBlow));
            if (TryGetView(out view))
                _universalBlow = new UniversalBlow(animationCommand, view, transforms);

            _stateCharacterKey.SetAbilityCommand(typeof(Declaration.GhostWolf));
            _stateCharacterKey.SetState(typeof(GhostWolf));
            if (TryGetView(out view))
                _ghostWolf = new GhostWolf(animationCommand, view, transforms);
        }

        protected override void InitializeTransitions(BaseState idleState)
        {
            _stateMachine.AddTransition(_unleashingRage, idleState, () => _unleashingRage.CanSkip);
            _stateMachine.AddTransition(_inductionCoilState, idleState, () => _inductionCoilState.CanSkip);
            _stateMachine.AddTransition(_attackStunState, idleState, () => _attackStunState.CanSkip);
            _stateMachine.AddTransition(_droneHammerState, idleState, () => _droneHammerState.CanSkip);
            _stateMachine.AddTransition(_swordRain, idleState, () => _swordRain.CanSkip);
            _stateMachine.AddTransition(_manaShield, idleState, () => _manaShield.CanSkip);
            _stateMachine.AddTransition(_armageddon, idleState, () => _armageddon.CanSkip);
            _stateMachine.AddTransition(_fury, idleState, () => _fury.CanSkip);
            _stateMachine.AddTransition(_universalBlow, idleState, () => _universalBlow.CanSkip);
            _stateMachine.AddTransition(_ghostWolf, idleState, () => _ghostWolf.CanSkip);
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

        public override void Armageddon()
        {
            _stateMachine.ChangeState(_armageddon);
        }

        public override void Fury()
        {
            _stateMachine.ChangeState(_fury);
        }

        public override void UniversalBlow()
        {
            _stateMachine.ChangeState(_universalBlow);
        }

        public override void GhostWolf()
        {
            _stateMachine.ChangeState(_ghostWolf);
        }
    }
}