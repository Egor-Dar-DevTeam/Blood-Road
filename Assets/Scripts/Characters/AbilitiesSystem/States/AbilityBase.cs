using Characters.Animations;
using Characters.Information.Structs;

namespace Characters.AbilitiesSystem.States
{
    public abstract class AbilityBase : BaseState
    {
        public bool CanSkip { get; protected set; }
        public AbilityBase(){}

        protected AbilityBase(IAnimationCommand animation, StateInfo stateInfo, VFXTransforms vfxTransforms) : base(animation, stateInfo, vfxTransforms)
        {
        }
    }
}