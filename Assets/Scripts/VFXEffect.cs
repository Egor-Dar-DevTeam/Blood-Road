using System;
using System.Collections;
using Characters;
using JetBrains.Annotations;
using UnityEngine;

public class VFXEffect : MonoBehaviour
{
    [SerializeField] [CanBeNull] private BaseCharacter character;
    public void SetLifeTime(float lifeTime)
    {
        StartCoroutine(Timer(lifeTime));
    }

    private IEnumerator Timer(float time)
    {
        yield return new WaitForSeconds(time);
        if (character != null)
        {
            character.ReceiveDamage(Int32.MaxValue);
            character.ReceiveDamage(Int32.MaxValue);
        }
        else Destroy(gameObject);
    }

    private void OnDestroy()
    {
        Destroy(gameObject);
    }
}