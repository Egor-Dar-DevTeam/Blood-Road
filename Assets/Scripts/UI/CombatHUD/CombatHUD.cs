using UnityEngine;
using UnityEngine.UI;

namespace UI.CombatHUD
{
    public delegate void UpdateEnergyDelegate(float energy);
    public delegate void UpdateHealthDelegate(float health);
    public delegate void UpdateShieldDelegate(float shield);
    public class CombatHUD : MonoBehaviour
    {
        [SerializeField] private Slider healthSlider;
        [SerializeField] private Slider shieldSlider;
        [SerializeField] private Slider energySlider;
        

        public void SetHealth(float health)
        {
            var t = Mathf.InverseLerp(0, 200, health);
            healthSlider.value = Mathf.Lerp(0, 1, t);
        }

        public void SetShield(float shield)
        {
            var t = Mathf.InverseLerp(0, 100, shield);
            shieldSlider.value = Mathf.Lerp(0, 1, t);
        }

        public void SetEnergy(float energy)
        {
            var t = Mathf.InverseLerp(0, 100, energy);
            energySlider.value = Mathf.Lerp(0, 1, t);
        }
        
    }
}