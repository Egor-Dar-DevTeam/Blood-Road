using System;
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
            var stun = new AttackStun(null, new StateInfo(), null);
            LibrarySystem.Ability.StaticAddEntity(typeof(Stun), new AbilitiesSystem.Declaration.StunAttack());
            LibrarySystem.Ability.StaticAddState(typeof(AbilitiesSystem.Declaration.StunAttack),stun.GetType());
            var droneHummer = new DroneHammer(null, new StateInfo(), null);
            LibrarySystem.Ability.StaticAddEntity(typeof(DroneHammer), new AbilitiesSystem.Declaration.DroneHammer());
            LibrarySystem.Ability.StaticAddState(typeof(AbilitiesSystem.Declaration.DroneHammer), droneHummer.GetType());
        }
    }
}