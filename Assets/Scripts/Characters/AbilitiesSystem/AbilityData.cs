using Better.UnityPatterns.Runtime.StateMachine;
using Characters.Animations;
using Characters.Player;
using JetBrains.Annotations;

namespace Characters.AbilitiesSystem
{
    public struct AbilityData
    {
        public VFXTransforms VFXTransforms { get; }
        public Impenetrable ImpenetrableDelegate { get; }
        public AbilitiesInfo AbilitiesInfo { get; }

        public StateMachine<BaseState> StateMachine { get; private set; }
        public IAnimationCommand AnimationCommand { get; private set; }
        public BaseState IdleState { get; private set; }
        public CharacterData CharacterData { get; }
        public Attack Attack { get; }
        public OverrideAttack OverrideAttack { get; }
        public SetAttackSpeed SetAttackSpeed { get; }

        public AbilityData(VFXTransforms vfxTransforms, AbilitiesInfo abilitiesInfo, Impenetrable impenetrable, CharacterData characterData
            ,[CanBeNull] Attack attack = null, [CanBeNull] OverrideAttack overrideAttack = null, [CanBeNull] SetAttackSpeed setAttackSpeed = null)
        {
            VFXTransforms = vfxTransforms;
            StateMachine = null;
            AnimationCommand = null;
            AbilitiesInfo = abilitiesInfo;
            IdleState = null;
            ImpenetrableDelegate = impenetrable;
            CharacterData = characterData;
            Attack = attack;
            OverrideAttack = overrideAttack;
            SetAttackSpeed = setAttackSpeed;
        }

        public void SetStateMachine(StateMachine<BaseState> stateMachine)
        {
            StateMachine = stateMachine;
        }

        public void SetAnimationCommand(IAnimationCommand animationCommand) => AnimationCommand = animationCommand;
        public void SetIdleState(BaseState state) => IdleState = state;
    }
}