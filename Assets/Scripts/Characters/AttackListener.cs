using Characters.AbilitiesSystem;
using Characters.EffectSystem;
using Characters.WeaponSystem;
using UnityEngine;

namespace Characters
{
    public delegate void AttackedAbility(Receiver receiver, IAbilityCommand data);

    public delegate void AttackedWeapon(Receiver receiver, Weapon data);
    public class AttackListener : MonoBehaviour
    {
        [SerializeField] private BaseCharacter character;
        private Sender _sender;

        private void Awake()
        {
            character.AttackAbility += Attacked;
            character.AttackWeapon += Attacked;
            _sender = character.Sender;
        }

        private void Attacked(Receiver receiver,Weapon weapon)
        {
            _sender.RegisterData(weapon.EffectData);
            _sender.RegisterReceiver(receiver);
        }

        private void Attacked(Receiver receiver, IAbilityCommand abilityCommand)
        {
            _sender.RegisterData(abilityCommand.GetEffectData());
            _sender.RegisterReceiver(receiver);
        }
    }
}