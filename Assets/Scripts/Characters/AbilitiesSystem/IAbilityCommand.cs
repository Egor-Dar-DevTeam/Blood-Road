using System;
using Characters.EffectSystem;

namespace Characters.AbilitiesSystem
{
    
    public interface IAbilityCommand{
        public EffectData EffectData { get; }
        public void Apply(Abilities script);
    }
}