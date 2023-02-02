using Characters;
using JetBrains.Annotations;
using UnityEngine;

public class DamagedTrigger : MonoBehaviour
{
    [SerializeField] private int damage;
    [CanBeNull] [SerializeField] private ParticleSystem particleSystem;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IInteractable enemy))
        {
            if (!enemy.IsPlayer())
            {
                enemy.ReceiveDamage(damage);
            }
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        
        if (other.TryGetComponent(out IInteractable enemy))
        {
            if (!enemy.IsPlayer())
            {
                enemy.ReceiveDamage(damage);
            }
        }
    }
}
