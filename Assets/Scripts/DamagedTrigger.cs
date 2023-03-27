using System.Collections.Generic;
using System.Threading.Tasks;
using Characters;
using UnityEngine;

public class DamagedTrigger : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private int waitFromActivated;
    [SerializeField] private int repeatActivate;
    private List<IInteractable> _interactables;

    private void Awake()
    {
        _interactables = new List<IInteractable>();
        Active();
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.gameObject.TryGetComponent(out IInteractable interactable)) return;
        if (_interactables.Contains(interactable)) return;
        if (interactable.IsPlayer()) return;
        if(interactable.HasCharacter())
            _interactables.Add(interactable);
    }

    private async void Active()
    {
        for (int i = 0; i < repeatActivate; i++)
        {
            await Task.Delay(waitFromActivated);
            foreach (var interactable in _interactables)
            {
                interactable.ReceiveDamage(damage);
            }
        }
    }
}