using System;
using System.Collections;
using System.Collections.Generic;
using Characters;
using UnityEngine;

public class DamagedTrigger : MonoBehaviour
{
    [SerializeField] private int damage;

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
}
