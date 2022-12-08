using UnityEngine;

public class ToggleActiveGameobjectsList : MonoBehaviour
{
    private bool currentState = false;

    public GameObject[] objectsList;

    
    public void ToggleActiveState()
    {
        foreach (GameObject i in objectsList)
        {
            i.SetActive(currentState);
        }
    if (currentState)
    {
        currentState = false;
    }else
    currentState = true;

    }
}
