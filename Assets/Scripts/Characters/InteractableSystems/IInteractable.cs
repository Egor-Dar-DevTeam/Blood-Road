using Characters.AbilitiesSystem;
using Characters.EffectSystem;
using Characters.Player;
using UnityEngine;

namespace Characters
{
    public interface IInteractable
    {
        public void ReceiveDamage(int value);
        public void SetOutline(bool value);
        public Transform GetObject();
        public bool IsPlayer();
        public DieDelegate GetDieCharacterDelegate();
        public event DieDelegate GetDieEvent;
        public  RemoveList GetRemoveList();
        public bool HasCharacter();
        public Receiver Receiver { get; }
    }
}