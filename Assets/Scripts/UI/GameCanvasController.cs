using Characters.InteractableSystems;
using Characters.Player;
using UI.AbilityChangedPanel;
using UnityEngine;

namespace UI
{
    public delegate void GamePanel();
    public class GameCanvasController : MonoBehaviour
    {
        [SerializeField] private CombatHUD.InfoCharactersResourceBars infoCharactersResourceBars;
        [SerializeField] private CanvasGroup combat;
        [SerializeField] private CanvasGroup death;
        [SerializeField] private CanvasGroup abilityChanged;
        [SerializeField] private PlayerController playerController;
        private IInit<AbilityTrigger> _initAbilityTrigger;
        private IInit<GamePanel> _initGamePanel;
        private AbilityTrigger abilityTrigger;
        private RechangePanel _rechangePanel;
        public UIDelegates UIDelegates => _uiDelegates;
        private UIDelegates _uiDelegates;


        private void Awake()
        {
            abilityTrigger = AbiltyChanged;
            _initAbilityTrigger = playerController;
            _initAbilityTrigger.Initialize(abilityTrigger);
            abilityChanged.gameObject.TryGetComponent(out _initGamePanel);
            _initGamePanel.Initialize(Game);
            _initGamePanel.Initialize(playerController.OnAbilityTrigger);
            var delegatesCharactersInfo = new UIDelegatesCharactersInfo();
            delegatesCharactersInfo.SetDelegates(infoCharactersResourceBars.SetEnergy,
                infoCharactersResourceBars.SetMana, infoCharactersResourceBars.SetHealth);
            _uiDelegates = delegatesCharactersInfo.Delegates();
            _rechangePanel = new RechangePanel();
            Game();
        }

        private void Game()
        {
            _rechangePanel.SetNewPanel(combat);
        }

        public void Death()
        {
            _rechangePanel.SetNewPanel(death);
        }

        private void AbiltyChanged()
        {
            _rechangePanel.SetNewPanel(abilityChanged);
            abilityChanged.TryGetComponent(out AbilityChanged changed);
            changed.Activate();
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