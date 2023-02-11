using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    public class GameCanvasController : MonoBehaviour
    {
        [FormerlySerializedAs("combatHUD")] [SerializeField] private CombatHUD.InfoCharactersResourceBars infoCharactersResourceBars;
        [SerializeField] private CanvasGroup combat;
        [SerializeField] private CanvasGroup death;
        private RechangePanel _rechangePanel;
        public UIDelegates UIDelegates => _uiDelegates;
        private UIDelegates _uiDelegates;
        

        private void Awake()
        {
            var delegatesCharactersInfo = new UIDelegatesCharactersInfo();
            delegatesCharactersInfo.SetDelegates(infoCharactersResourceBars.SetEnergy,
                infoCharactersResourceBars.SetMana, infoCharactersResourceBars.SetHealth);
            _uiDelegates = delegatesCharactersInfo.Delegates();
            _rechangePanel = new RechangePanel();
            _rechangePanel.SetNewPanel(combat);
        }

        public void Death()
        {
            _rechangePanel.SetNewPanel(death);
        }
        
    }

    public class RechangePanel
    {
        private CanvasGroup _currentCanvasGroup;

        public void SetNewPanel(CanvasGroup newPanel)
        {
            if (_currentCanvasGroup != null)
            {
                _currentCanvasGroup.alpha = 0;
                _currentCanvasGroup.interactable = false;
                _currentCanvasGroup.blocksRaycasts = false;
            }

            _currentCanvasGroup = newPanel;
            _currentCanvasGroup.alpha = 1;
            _currentCanvasGroup.interactable = true;
            _currentCanvasGroup.blocksRaycasts = true;
        }
    }
}