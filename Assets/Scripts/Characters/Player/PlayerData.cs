using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Characters.Player
{
    [Serializable]
    public class PlayerData
    {
        [SerializeField] private float health;
        [SerializeField] private float shield;
        [SerializeField] private float energy;

        public float Health => health;
        public float Shield => shield;
        public float Energy => energy;

        public void UseEnergy()
        {
            if(energy<=0) return;
            energy-=25;
            if (energy <= 0) energy = 0;
        }

        public void Damage(float value)
        {
            var shieldCoeff = 0.8f;
            var dmgToShield = 0f;
           if(shield!=0) dmgToShield = value * shieldCoeff;
            var dmgToHealt = value * (1 - shieldCoeff);
            shield = Mathf.Clamp(shield - dmgToShield, 0, 1000);
            health = Mathf.Clamp(health - dmgToHealt, 0, 1000);

        }
    }
}