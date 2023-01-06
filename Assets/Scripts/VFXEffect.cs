using System.Collections;
using UnityEngine;

public class VFXEffect : MonoBehaviour
{
    private  void Awake()
    {
        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        Destroy(gameObject);
    }
}