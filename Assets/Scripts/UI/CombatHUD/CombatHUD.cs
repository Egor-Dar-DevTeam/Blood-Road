using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.CombatHUD
{
    public delegate void UpdateEnergyDelegate(float energy);

    public delegate void UpdateHealthDelegate(float health);

    public delegate void UpdateManaDelegate(float mana);

    public class CombatHUD : MonoBehaviour
    {
        [SerializeField] private Slider healthSlider;
        [SerializeField] private Slider manaSlider;
        [SerializeField] private Slider energySlider;


        public void SetHealth(float health)
        {
            var t = Mathf.InverseLerp(0, 200, health);
            healthSlider.value = Mathf.Lerp(0, 1, t);
        }

        public void SetMana(float mana)
        {
            var t = Mathf.InverseLerp(0, 100, mana);
            manaSlider.value = Mathf.Lerp(0, 1, t);
        }

        public void SetEnergy(float energy)
        {
            var t = Mathf.InverseLerp(0, 100, energy);
            energySlider.value = Mathf.Lerp(0, 1, t);
        }
    }
}