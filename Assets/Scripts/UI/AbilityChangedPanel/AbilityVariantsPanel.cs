using Characters.AbilitiesSystem.Ability;
using Characters.InteractableSystems;
using UI.CombatHUD;
using UnityEngine;

namespace UI.AbilityChangedPanel
{
    public class AbilityVariantsPanel : MonoBehaviour, IInit<GamePanel>
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private AbilityInfo[] abilitiesInfo;
        [SerializeField] private AbilitiesData abilitiesData;
        private AbilitiesButtons _abilitiesButtons;
        private event GamePanel _gamePanel;
        public CanvasGroup CanvasGroup => canvasGroup;

        public void SetAbilitiesButtons(AbilitiesButtons abilitiesButtons)
        {
            _abilitiesButtons = abilitiesButtons;
        }

        public void SetPanelsInfo()
        {
            var abilitiesSO = abilitiesData.GetCopy();
            var actualAbilities = _abilitiesButtons.GetCopy();
            foreach (AbilitySO ability in actualAbilities)
            {
                abilitiesSO.Remove(ability);
            }

            foreach (var abilityInfo in abilitiesInfo)
            {
                var ability = abilitiesSO[Random.Range(1, abilitiesSO.Count)];

                abilitiesSO.Remove(ability);
                ability.Initialize();
                abilityInfo.SetInfo(ability.AbilityUIInfo);
                abilityInfo.Button.onClick.RemoveAllListeners();
                abilityInfo.Button.onClick.AddListener(() => SetNewAbility(ability));
            }
        }

        private void SetNewAbility(AbilitySO abilitySo)
        {
            _abilitiesButtons.AddAbility(abilitySo);
            _gamePanel?.Invoke();
        }

        public void Initialize(GamePanel subscriber)
        {
            _gamePanel += subscriber;
        }
    }
}