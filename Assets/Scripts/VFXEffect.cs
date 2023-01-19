using System.Collections;
using UnityEngine;

public class VFXEffect : MonoBehaviour
{
    public void SetLifeTime(float lifeTime)
    {
        StartCoroutine(Timer(lifeTime));
    }

    private IEnumerator Timer(float tine)
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        Destroy(gameObject);
    }
}