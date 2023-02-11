using Characters;
using UnityEngine;

namespace Interaction
{
    public class FinishTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IInteractable interactable))
            {
                interactable.Finish();
            }
        }
    }
}