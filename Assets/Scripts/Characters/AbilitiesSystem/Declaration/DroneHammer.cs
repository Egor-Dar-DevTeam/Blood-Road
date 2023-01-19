namespace Characters.AbilitiesSystem.Declaration
{
    public class DroneHammer : IAbilityCommand{
        public void Apply(Abilities script){
            script.DroneHammer();
        }
    }
}