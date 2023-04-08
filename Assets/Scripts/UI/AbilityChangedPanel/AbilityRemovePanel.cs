using Characters.InteractableSystems;
using MapSystem;
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

        private void RemoveAbility(Item abilitySo)
        {
            _abilitiesButtons.RemoveAbility(abilitySo);
            _abilityVariants?.Invoke();
        }
        
        public void ViewActualAbilities()
        {
            var abilityList = _abilitiesButtons.GetCopy();
            for (int i = 0; i < abilitiesInfo.Length; i++)
            {
                var info = abilityList[i].UIInfo;
                var ability = abilityList[i];
                abilitiesInfo[i].SetInfo(info);
                abilitiesInfo[i].Button.onClick.RemoveAllListeners();
                abilitiesInfo[i].Button.onClick.AddListener((() => RemoveAbility(ability)));
            }
        }
        public void Subscribe(UnityAction subscriber)
        {
            _abilityVariants = subscriber;
        }

        public void Unsubscribe(UnityAction unsubscriber)
        {
            _abilityVariants = null;
        }
    }
}