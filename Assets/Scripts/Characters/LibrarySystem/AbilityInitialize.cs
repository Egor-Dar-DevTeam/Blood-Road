using System;
using Characters.AbilitiesSystem;
using Characters.AbilitiesSystem.States;
using Characters.Information.Structs;
using UnityEngine;

namespace Characters.LibrarySystem
{
    public class AbilityInitialize : MonoBehaviour
    {
        private void Awake()
        {
            Ability.Initialize();
            InitializeLibrary();
        }

        private void InitializeLibrary()
        {
            AddToLibrary(new DroneHammer(null, new StateInfo(), null), new AbilitiesSystem.Declaration.DroneHammer());
           AddToLibrary(new AttackStun(null, new StateInfo(), null), new AbilitiesSystem.Declaration.StunAttack());
           AddToLibrary(new SwordRain(null, new StateInfo(), null), new AbilitiesSystem.Declaration.SwordRain());
           AddToLibrary(new InductionCoil(null, new StateInfo(), null), new AbilitiesSystem.Declaration.InductionCoil());
        }
        
        private void AddToLibrary(AbilityBase abilityBase, IAbilityCommand abilityCommand)
        {
            LibrarySystem.Ability.StaticAddEntity(abilityBase.GetType(), abilityCommand);
            LibrarySystem.Ability.StaticAddState(abilityCommand.GetType(), abilityBase.GetType());
        }
    }
}