using System;
using Characters.AbilitiesSystem;
using Characters.EffectSystem;
using Characters.LibrarySystem;
using Characters.WeaponSystem;
using UnityEngine;

namespace Characters
{
    public delegate void AttackedAbility(Receiver receiver, IAbilityCommand data);

    public delegate void AttackedWeapon(Receiver receiver, Weapon data);
    public class AttackListener : MonoBehaviour
    {
        [SerializeField]private Sender _sender;
        [SerializeField] private BaseCharacter character;

        private void Awake()
        {
            character.AttackAbility += Attacked;
            character.AttackWeapon += Attacked;
        }

        private void Attacked(Receiver receiver,Weapon weapon)
        {
            _sender.RegisterReceiver(receiver);
            _sender.RegisterData(weapon.EffectData);
        }

        private void Attacked(Receiver receiver, IAbilityCommand abilityCommand)
        {
            _sender.RegisterReceiver(receiver);
            _sender.RegisterData(abilityCommand.EffectData);
            receiver.RegisterAnimator(Ability.StaticToEntityID(abilityCommand));
        }
    }
}