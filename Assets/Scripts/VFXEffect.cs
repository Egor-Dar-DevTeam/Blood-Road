using System;
using System.Collections;
using Characters;
using Characters.EffectSystem;
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
            character.WeaponAttack(new EffectData(Int32.MaxValue, 0,0,0,0,null));
        }
        else Destroy(gameObject);
    }

    private void OnDestroy()
    {
        Destroy(gameObject);
    }
}