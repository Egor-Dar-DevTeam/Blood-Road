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

        public void Subscribe(GamePanel subscriber)
        {
            _gamePanel += subscriber;
        }

        public void Unsubscribe(GamePanel unsubscriber)
        {
            _gamePanel -= unsubscriber;

        }

        public void Subscribe(UnityAction subscriber)
        {
            _removeAbilityPanel += subscriber;
        }

        public void Unsubscribe(UnityAction unsubscriber)
        {
            _removeAbilityPanel -= unsubscriber;
        }
    }
}