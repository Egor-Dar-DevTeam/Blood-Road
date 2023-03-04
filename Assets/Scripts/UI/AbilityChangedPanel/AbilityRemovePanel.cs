using Characters.AbilitiesSystem.Ability;
using Characters.InteractableSystems;
using UI.CombatHUD;
using UnityEngine;
using UnityEngine.Events;

namespace UI.AbilityChangedPanel
{
    public class AbilityRemovePanel : MonoBehaviour, IInit<UnityAction>
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private AbilityInfo[] abilitiesInfo;
        private AbilitiesButtons _abilitiesButtons;
        private UnityAction _abilityVariants;
        public CanvasGroup CanvasGroup => canvasGroup;

        public void SetAbilitiesButtons(AbilitiesButtons abilitiesButtons)
        {
            _abilitiesButtons = abilitiesButtons;
        }

        public void ViewActualAbilities()
        {
            var abilitySOList = _abilitiesButtons.GetCopy();
            for (int i = 0; i < abilitiesInfo.Length; i++)
            {
                var info = abilitySOList[i].AbilityUIInfo;
                var abilitySO = abilitySOList[i];
                abilitiesInfo[i].SetInfo(info);
                abilitiesInfo[i].Button.onClick.RemoveAllListeners();
                abilitiesInfo[i].Button.onClick.AddListener((() => RemoveAbility(abilitySO)));
            }
        }

        private void RemoveAbility(AbilitySO abilitySo)
        {
            _abilitiesButtons.RemoveAbility(abilitySo);
            _abilityVariants?.Invoke();
        }

        public void Initialize(UnityAction subscriber)
        {
            _abilityVariants = subscriber;
        }
    }
}