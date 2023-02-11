using UnityEngine;
using UnityEngine.UI;

namespace UI.CombatHUD
{
    public delegate void UpdateEnergyDelegate(float value, float maxValue);

    public delegate void UpdateHealthDelegate(float value, float maxValue);

    public delegate void UpdateManaDelegate(float value, float maxValuea);

    public class InfoCharactersResourceBars : MonoBehaviour
    {
        [SerializeField] private Slider healthSlider;
        [SerializeField] private Slider manaSlider;
        [SerializeField] private Slider energySlider;


        public void SetHealth(float value, float maxValue)
        {
            if(healthSlider==null) return;
            var t = Mathf.InverseLerp(0, maxValue, value);
            healthSlider.value = Mathf.Lerp(0, 1, t);
        }

        public void SetMana(float value, float maxValue)
        {
            if(manaSlider==null) return;
            var t = Mathf.InverseLerp(0, maxValue, value);
            manaSlider.value = Mathf.Lerp(0, 1, t);
        }

        public void SetEnergy(float value, float maxValuey)
        {
            if(energySlider==null) return;
            var t = Mathf.InverseLerp(0, maxValuey, value);
            energySlider.value = Mathf.Lerp(0, 1, t);
        }
    }
}