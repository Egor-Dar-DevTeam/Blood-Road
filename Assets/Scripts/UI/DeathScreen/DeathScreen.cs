using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathScreen : MonoBehaviour
{
    [SerializeField] private Button restart;

    private void Awake()
    {
        restart.onClick.AddListener((() => SceneManager.LoadScene(0)));
    }
}
