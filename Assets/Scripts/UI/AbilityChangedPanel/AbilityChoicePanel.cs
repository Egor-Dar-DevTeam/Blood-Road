using Characters.InteractableSystems;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.AbilityChangedPanel
{
    public class AbilityChoicePanel: MonoBehaviour , IInit<GamePanel>, IInit<UnityAction>
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _removeButton;
        private event GamePanel _gamePanel;
        private UnityAction _removeAbilityPanel;
        public CanvasGroup CanvasGroup => canvasGroup;
        private void Start()
        {
            _continueButton.onClick.AddListener(OnContinue);
            _removeButton.onClick.AddListener(OnRemoveAbility);
        }

        private void OnContinue()
        {
            _gamePanel?.Invoke();
        }

        private void OnRemoveAbility()
        {
            _removeAbilityPanel?.Invoke();
        }

        public void Initialize(GamePanel subscriber)
        {
            _gamePanel += subscriber;
        }

        public void Initialize(UnityAction subscriber)
        {
            _removeAbilityPanel += subscriber;
        }
    }
}