using System.Collections;
using UnityEngine;

public class VFXEffect : MonoBehaviour
{
    public void SetLifeTime(float lifeTime)
    {
        StartCoroutine(Timer(lifeTime));
    }

    private IEnumerator Timer(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        Destroy(gameObject);
    }
}