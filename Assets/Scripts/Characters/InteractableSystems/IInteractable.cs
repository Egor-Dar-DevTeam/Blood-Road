using Characters.AbilitiesSystem;
using Characters.Player;
using UnityEngine;

namespace Characters
{
    public interface IInteractable
    {
        public IInteractableAbility InteractableAbility { get; }
        public void ReceiveDamage(int value);
        public void SetOutline(Material outline);
        public Transform GetObject();
        public bool IsPlayer();
        public DieDelegate GetDieCharacterDelegate();
        public event DieDelegate GetDieEvent;
        public bool HasCharacter();
    }
}