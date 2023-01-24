using Characters.Animations;
using Characters.Information.Structs;

namespace Characters.AbilitiesSystem.States
{
    public abstract class AbilityState : BaseState
    {
        public bool CanSkip { get; protected set; }
        protected AbilityState(IAnimationCommand animation, StateInfo stateInfo, VFXTransforms vfxTransforms) : base(animation, stateInfo, vfxTransforms)
        {
        }
    }
}