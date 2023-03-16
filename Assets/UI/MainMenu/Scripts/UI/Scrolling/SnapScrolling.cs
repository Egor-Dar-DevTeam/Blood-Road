using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UI;
using UI.Scrolling;
using UnityEngine;
using UnityEngine.UI;

public class SnapScrolling : MonoBehaviour, IInit<MainPanelDelegate>, IInit<SetInfoDelegate>
{
    [Range(1, 50)] [Header("controllers")] [SerializeField]
    private int panelCount;

    [SerializeField] private List<LevelInfo> levelInfos;  

    [Range(0, 500)] [SerializeField] private int panelOffset;

    [Header("Scrolling objects")] [SerializeField]
    private ScrollPanel panelPrefab;

    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private float snapSpeed;
    [SerializeField] private float scaleOffset;
    [SerializeField] private float scaleSpeed;
    [SerializeField] private CanvasGroup locationPanel;
    [SerializeField] private TextMeshProUGUI locationNames;
    [SerializeField] private Button enterbutton;
    

    private ScrollPanel[] _panels;
    private Vector2[] _panelsPosition;
    private RectTransform _contentRect;
    private int _selectedPanelID;
    private bool _isScrolling;
    private Vector2 _contentVector;
    private Vector2[] _panelsScale;
    private event MainPanelDelegate _mainPanel;
    private event SetInfoDelegate _setInfo;

    private void Start()
    {
        _panels = new ScrollPanel[panelCount];
        _panelsPosition = new Vector2[panelCount];
        _contentRect = GetComponent<RectTransform>();
        _panelsScale = new Vector2[panelCount];
        enterbutton.onClick.AddListener(()=>_mainPanel.Invoke());
        enterbutton.onClick.AddListener(()=>_setInfo.Invoke(levelInfos[_selectedPanelID]));

        for (int i = 0; i < panelCount; i++)
        {
            _panels[i] = Instantiate(panelPrefab, transform, false);
            _panels[i].SetLocationInfo(levelInfos[i]);
            if (i == 0) continue;
            _panels[i].transform.localPosition =
                new Vector2(
                    _panels[i - 1].transform.localPosition.x + panelPrefab.GetComponent<RectTransform>().sizeDelta.x +
                    panelOffset, _panels[i].transform.localPosition.y);

            _panelsPosition[i] = -_panels[i].transform.localPosition;
        }
    }

    public void Initialize(MainPanelDelegate subscriber)
    {
        _mainPanel += subscriber;
    }

    private void FixedUpdate()
    {
        if (_contentRect.anchoredPosition.x >= _panelsPosition[0].x && !_isScrolling ||
            _contentRect.anchoredPosition.x <= _panelsPosition[_panelsPosition.Length - 1].x && !_isScrolling)
        {
            scrollRect.inertia = false;
            ShowLocationName(true);
            _panels[_selectedPanelID].SetTMProName(locationNames);
        }

        float nearestPosition = float.MaxValue;
        for (int i = 0; i < panelCount; i++)
        {
            float distance = Math.Abs(_contentRect.anchoredPosition.x - _panelsPosition[i].x);
            if (distance < nearestPosition)
            {
                nearestPosition = distance;
                _selectedPanelID = i;
            }

            float scale = Mathf.Clamp(1 / (distance / panelOffset) * scaleOffset, 0.5f, 1f);
            _panels[i].transform.DOScale(new Vector3(scale, scale, 1), scaleSpeed * Time.fixedDeltaTime).OnComplete(
                () => { _panelsScale[i] = _panels[i].transform.localScale; });
        }

        float scrollVelocity = Mathf.Abs(scrollRect.velocity.x);
        if (scrollVelocity < 400 && !_isScrolling)
        {
            scrollRect.inertia = false;
            ShowLocationName(true);
            _panels[_selectedPanelID].SetTMProName(locationNames);
        }

        if (_isScrolling || scrollVelocity > 400) return;
        _contentVector.x =
            Mathf.SmoothStep(_contentRect.anchoredPosition.x, _panelsPosition[_selectedPanelID].x,
                snapSpeed * Time.fixedDeltaTime);
        _contentRect.anchoredPosition = _contentVector;
    }

    public void Scroll(bool scroll)
    {
        _isScrolling = scroll;
        ShowLocationName();
        if (scroll) scrollRect.inertia = true;
    }

    private void ShowLocationName(bool value = false)
    {
        if (value)
        {
            locationPanel.DOFade(1, 0.3f);
        }
        else
        {
            locationPanel.DOFade(0, 0.3f);
        }
    }


    public void Initialize(SetInfoDelegate subscriber)
    {
        _setInfo += subscriber;
    }
}