using UnityEngine;

namespace Characters
{
    public interface IInteractable
    {
        public void SetOutline(Material outline);
        public Transform GetObject();
    }
}