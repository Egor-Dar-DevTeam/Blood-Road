using Better.UnityPatterns.Runtime.StateMachine;
using Characters.Animations;
using Characters.Player;

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


        public AbilityData(VFXTransforms vfxTransforms, AbilitiesInfo abilitiesInfo, Impenetrable impenetrable)
        {
            VFXTransforms = vfxTransforms;
            StateMachine = null;
            AnimationCommand = null;
            AbilitiesInfo = abilitiesInfo;
            IdleState = null;
            ImpenetrableDelegate = impenetrable;
        }

        public void SetStateMachine(StateMachine<BaseState> stateMachine)
        {
            StateMachine = stateMachine;
        }

        public void SetAnimationCommand(IAnimationCommand animationCommand) => AnimationCommand = animationCommand;
        public void SetIdleState(BaseState state) => IdleState = state;
    }
}