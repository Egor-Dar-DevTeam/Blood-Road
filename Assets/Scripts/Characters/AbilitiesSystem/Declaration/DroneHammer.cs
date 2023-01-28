using Characters.EffectSystem;

namespace Characters.AbilitiesSystem.Declaration
{
    public class DroneHammer : IAbilityCommand
    {
        public EffectData EffectData => new EffectData();

        public void Apply(Abilities script){
            script.DroneHammer();
        }
    }
}