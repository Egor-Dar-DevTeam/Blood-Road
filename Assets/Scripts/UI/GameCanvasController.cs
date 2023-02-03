using UI.CombatHUD;
using UnityEngine;

namespace UI
{
    public class GameCanvasController : MonoBehaviour
    {
        [SerializeField] private CombatHUD.CombatHUD combatHUD;
        [SerializeField] private CanvasGroup combat;
        [SerializeField] private CanvasGroup death;
        private RechangePanel _rechangePanel;
        public UIDelegates UIDelegates => _uiDelegates;
        private UIDelegates _uiDelegates;

        
        private UpdateEnergyDelegate _updateEnergyDelegate;
        private UpdateManaDelegate _updateManaDelegate;
        private UpdateHealthDelegate _updateHealthDelegate;

        private void Awake()
        {
            _updateManaDelegate = combatHUD.SetMana;
            _updateHealthDelegate = combatHUD.SetHealth;
            _updateEnergyDelegate = combatHUD.SetEnergy;
            _uiDelegates = new UIDelegates(_updateEnergyDelegate, _updateManaDelegate, _updateHealthDelegate);
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