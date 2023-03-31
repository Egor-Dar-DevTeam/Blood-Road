using System.Collections.Generic;
using System.Threading.Tasks;
using Characters;
using Characters.Player.States;
using UnityEngine;

public class DamagedTrigger : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private int waitFromActivated;
    [SerializeField] private int repeatActivate;
    [SerializeField] private ExplosionParameters explosionParameters;

    private List<IInteractable> _interactables;
    private const float DEFAULT_SPEED = 25f;
    private const float DEFAULT_RADIUS = 8f;
    private void Awake()
    {
        _interactables = new List<IInteractable>();
        Active();
    }
    private ExplosionParameters Validate(ExplosionParameters explosionParameters)
    {
        explosionParameters.Radius = explosionParameters.Radius == 0f ? DEFAULT_RADIUS : explosionParameters.Radius;
        explosionParameters.Speed = explosionParameters.Speed == 0f ? DEFAULT_SPEED : explosionParameters.Speed;
        return explosionParameters;
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
                interactable.GetRecoil(transform.position, Validate(explosionParameters));
            }
        }
    }
}