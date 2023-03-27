using Characters.AbilitiesSystem.States;
using Characters.Animations;
using Characters.InteractableSystems;
using Characters.Player;

namespace Characters.AbilitiesSystem
{
    public class Player : Abilities
    {
        private Attack _attack;
        private OverrideAttack _overrideAttack;
        private SetAttackSpeed _setAttackSpeed;
        private CharacterData _characterData;
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
            _attack = abilityData.Attack;
            _overrideAttack = abilityData.OverrideAttack;
            _setAttackSpeed = abilityData.SetAttackSpeed;
            _characterData = abilityData.CharacterData;
            CreateStates(abilityData.AnimationCommand, abilityData.VFXTransforms);
            InitializeTransitions(abilityData.IdleState);
            IInit<Impenetrable> initImpenerable = _manaShield;
            initImpenerable.Subscribe(abilityData.ImpenetrableDelegate);
        }

        protected override void CreateStates(IAnimationCommand animationCommand, VFXTransforms transforms)
        {
            _stunState = new Stun(animationCommand, _info.GetState(typeof(Stun)), transforms);
            _attackStunState = new AttackStun(animationCommand, _info.GetState(typeof(AttackStun)), transforms);
            _droneHammerState = new DroneHammer(animationCommand, _info.GetState(typeof(DroneHammer)), transforms);
            _swordRain = new SwordRain(animationCommand, _info.GetState(typeof(SwordRain)), transforms);
            _inductionCoilState = new InductionCoil(animationCommand, _info.GetState(typeof(InductionCoil)), transforms);
            _manaShield = new ManaShield(animationCommand, _info.GetState(typeof(ManaShield)), transforms);
            _unleashingRage = new UnleashingRage(animationCommand, _info.GetState(typeof(UnleashingRage)), transforms);
            _armageddon = new Armageddon(animationCommand, _info.GetState(typeof(Armageddon)), transforms);
            _fury = new Fury(animationCommand, _info.GetState(typeof(Fury)), transforms, _characterData);
            _universalBlow = new UniversalBlow(animationCommand, _info.GetState(typeof(UniversalBlow)), transforms,
                _characterData, _attack, _overrideAttack, _setAttackSpeed);
            _ghostWolf = new GhostWolf(animationCommand, _info.GetState(typeof(GhostWolf)), transforms);

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