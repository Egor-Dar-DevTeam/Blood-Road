using Characters.AbilitiesSystem.Ability;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.AbilityChangedPanel
{
    public class AbilityInfo : MonoBehaviour
    {
        [SerializeField] private Button button;
        [Space] [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI name;
        [SerializeField] private TextMeshProUGUI description;

        public Button Button => button;

        public void SetInfo(AbilityUIInfo abilityUIInfo)
        {
            image.sprite = abilityUIInfo.Sprite;
            name.text = abilityUIInfo.Name;
            description.text = abilityUIInfo.Description;
        }
    }
}