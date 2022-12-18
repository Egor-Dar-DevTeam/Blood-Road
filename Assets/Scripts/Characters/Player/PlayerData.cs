using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Characters.Player
{
    [Serializable]
    public class PlayerData
    {
        [SerializeField] private int health;
        [SerializeField] private int shield;
        [SerializeField] private int energy;

        public int Health => health;
        public int Shield => shield;
        public int Energy => energy;

        public void UseEnergy()
        {
            if(energy<=0) return;
            energy-=25;
            if (energy <= 0) energy = 0;
        }
    }
}