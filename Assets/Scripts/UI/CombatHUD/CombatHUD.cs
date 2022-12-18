using System;
using Characters.InteractableSystems;
using Characters.Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI.CombatHUD
{
    public delegate void UpdateEnergyDelegate(int energy);
    public delegate void UpdateHealthDelegate(int health);
    public delegate void UpdateShieldDelegate(int shield);
    public class CombatHUD : MonoBehaviour
    {
        [SerializeField] private Slider healthSlider;
        [SerializeField] private Slider shieldSlider;
        [SerializeField] private Slider energySlider;
        

        public void SetHealth(int health)
        {
            var t = Mathf.InverseLerp(0, 100, health);
            healthSlider.value = Mathf.Lerp(0, 1, t);
        }

        public void SetShield(int shield)
        {
            var t = Mathf.InverseLerp(0, 100, shield);
            shieldSlider.value = Mathf.Lerp(0, 1, t);
        }

        public void SetEnergy(int energy)
        {
            var t = Mathf.InverseLerp(0, 100, energy);
            energySlider.value = Mathf.Lerp(0, 1, t);
        }
        
    }
}