using UnityEngine;

namespace Characters
{
    public interface IInteractable
    {
        public void ReceiveDamage(float value);
        public void SetOutline(Material outline);
        public Transform GetObject();
        public bool IsPlayer();
    }
}