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
        public  RemoveList GetRemoveList();
        public bool HasCharacter();
        public Receiver Receiver { get; }

    }

    public interface ITriggerable
    {
        public void Finish();
        public void AbilityTrigger();
        public void AddMoney(int value);
    }
}