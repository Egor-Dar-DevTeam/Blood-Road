using Characters.EffectSystem;

namespace Characters.AbilitiesSystem.Declaration
{
    public class SwordRain : IAbilityCommand
    {
        public EffectData EffectData { get; }

        public void Apply(Abilities script)
        {
            script.SwordRain();
        }
    }
}