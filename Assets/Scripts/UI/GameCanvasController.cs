using System;
using Banks;
using Characters;
using Characters.InteractableSystems;
using Characters.Player;
using TMPro;
using UI.AbilityChangedPanel;
using UI.CombatHUD;
using UnityEngine;

namespace UI
{
    public delegate void GamePanel();

    public class GameCanvasController : MonoBehaviour
    {
        [SerializeField] private CanvasGroup combat;
        [SerializeField] private CanvasGroup death;
        [SerializeField] private CanvasGroup abilityChanged;
        [SerializeField] private PlayerController playerController;
        [SerializeField] private ResourceMediator resourceMediator;
        private IInit<AbilityTrigger> _initAbilityTrigger;
        private IInit<GamePanel> _initGamePanel;
        private AbilityTrigger abilityTrigger;
        private RechangePanel _rechangePanel;


        private void Awake()
        {
            resourceMediator.SetCharacter(playerController);
            abilityTrigger = AbiltyChanged;
            _initAbilityTrigger = playerController;
            _initAbilityTrigger.Subscribe(abilityTrigger);
            abilityChanged.gameObject.TryGetComponent(out _initGamePanel);
            _initGamePanel.Subscribe(Game);
            _initGamePanel.Subscribe(playerController.OnAbilityTrigger);
            _rechangePanel = new RechangePanel();
            Game();
        }

        private void Start()
        {
            resourceMediator.Subscribe();
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

        private void OnDestroy()
        {
            resourceMediator.Unsubscribe();
        }
    }

    [Serializable]
    public class ResourceMediator
    {
        [SerializeField] private ResourceSlider health;
        [SerializeField] private ResourceSlider mana;
        [SerializeField] private ResourceSlider energy;
        [SerializeField] private TextMeshProUGUI moneyText;
        private BaseCharacter _character;
        private ICharacterDataSubscriber _characterDataSubscriber => _character.CharacterDataSubscriber;
        private IInit<GetValue> _initGetValue;

        public void SetCharacter(BaseCharacter character)
        {
            _character = character;
            var characterPlayer = (PlayerController)_character;
            _initGetValue = characterPlayer.MoneyBankDelegates.InitGetValue;
        }

        public void Subscribe()
        {
            _characterDataSubscriber.HealthEvent += health.SetValue;
            _characterDataSubscriber.ManaEvent += mana.SetValue;
            _characterDataSubscriber.EnergyEvent += energy.SetValue;
            _initGetValue.Subscribe((value)=>moneyText.text = value.ToString() );

        }

        public void Unsubscribe()
        {
            _characterDataSubscriber.HealthEvent -= health.SetValue;
            _characterDataSubscriber.ManaEvent -= mana.SetValue;
            _characterDataSubscriber.EnergyEvent -= energy.SetValue;
            _initGetValue.Unsubscribe((value)=>moneyText.text = value.ToString() );
        }
    }
}