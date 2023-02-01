using Characters.EffectSystem;

namespace Characters.AbilitiesSystem.Declaration
{
    public class InductionCoil : IAbilityCommand
    {
        public EffectData EffectData { get; }
        public void Apply(Abilities script)
        {
            script.InductionCoin();
        }
    }
}