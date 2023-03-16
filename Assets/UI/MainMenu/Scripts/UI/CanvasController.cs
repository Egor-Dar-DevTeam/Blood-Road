using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    [SerializeField] private CanvasGroup scrollPanel;
    [SerializeField] private Button scrollButton;
    [SerializeField] private MainPanel mainPanel;
    [SerializeField] private SnapScrolling scroll;

    private ChangePanel _changePanel;
    private MainPanelDelegate _mainPanelDelegate;
    

    private void Start()
    {
        scroll.TryGetComponent(out IInit<MainPanelDelegate> initMain);
        _mainPanelDelegate = OnMainPanel;
        initMain.Initialize(_mainPanelDelegate);
        _changePanel = new ChangePanel();
        OnMainPanel();
        scrollButton.onClick.AddListener(OnScrollPanel);
        scroll.TryGetComponent(out IInit<SetInfoDelegate> initSetInfo);
        initSetInfo.Initialize(mainPanel.SetInfoDelegate);
        
    }
    
    public void OnScrollPanel()
    {
        _changePanel.SetPanel(scrollPanel);
    }

   public void OnMainPanel()
    {
        _changePanel.SetPanel(mainPanel.CanvasController);
    }

   
}

public delegate void MainPanelDelegate(); 
