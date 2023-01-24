namespace Characters.AbilitiesSystem.Declaration
{
    public class Stun : IAbilityCommand
    {
        public void Apply(Abilities script){
            script.Stun();
        }
    }
}