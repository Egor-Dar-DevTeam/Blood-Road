using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainPanel : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI progress, locationName;
    [SerializeField] private CanvasGroup canvasController;
    public CanvasGroup CanvasController => canvasController;

    public SetInfoDelegate SetInfoDelegate;

    private void Start()
    {
        playButton.onClick.AddListener(OnPlayButton);
        SetInfoDelegate = SetInfo;
    }

    private void OnPlayButton()
    {
        SceneManager.LoadScene("Game");
    }


    private void SetInfo(LevelInfo levelInfo)
    {
        var locationInfo = levelInfo.GetCopy();
        locationName.text = locationInfo.Name;
        image.sprite = locationInfo.Sprite;
        progress.text = locationInfo.Progress;
    }
}

public delegate void SetInfoDelegate(LevelInfo levelInfo);