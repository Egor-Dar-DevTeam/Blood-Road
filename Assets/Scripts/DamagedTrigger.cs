using System.Collections.Generic;
using System.Threading.Tasks;
using Characters;
using Characters.EffectSystem;
using UnityEngine;

public class DamagedTrigger : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private int waitFromActivated;
    [SerializeField] private int repeatActivate;
    [SerializeField] private bool drone;
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
        if (interactable.HasCharacter())
            _interactables.Add(interactable);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.TryGetComponent(out IInteractable interactable)) return;
        if (interactable.IsPlayer()) return;
        if (drone) interactable.TakeDamage(new EffectData(damage, 0, 0, 0, 0, null));
        if (_interactables.Contains(interactable)) return;
        if (interactable.HasCharacter())
            _interactables.Add(interactable);
    }

    private async void Active()
    {
        for (int i = 0; i < repeatActivate; i++)
        {
            await Task.Delay(waitFromActivated);
            foreach (var interactable in _interactables)
            {
                interactable.TakeDamage(new EffectData(damage, 0, 0, 0, 0, null));
            }
        }
    }
}