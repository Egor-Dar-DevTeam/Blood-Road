using Characters.Animations;
using MapSystem.Structs;

namespace Characters.AbilitiesSystem.States
{
    public abstract class AbilityBase : BaseState
    {
        public bool CanSkip { get; protected set; }
        public AbilityBase(){}

        protected AbilityBase(IAnimationCommand animation, View view, VFXTransforms vfxTransforms) : base(animation, view, vfxTransforms)
        {
        }
    }
}