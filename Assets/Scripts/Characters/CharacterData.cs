using System;
using Unity.Mathematics;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Characters.Player
{
    public delegate void DieDelegate();
    [Serializable]
    public class CharacterData
    {
        [SerializeField] private int health;
        [SerializeField] private int shield;
        [SerializeField] private int energy;
        [SerializeField] private int mana;
        [SerializeField] protected int damage;

        public CharacterData(int health, int shield, int energy, int mana, int damage)
        {
            this.health = health;
            this.shield = shield;
            this.energy = energy;
            this.mana = mana;
            this.damage = damage;
        }

        public CharacterData Copy()
        {
            return new CharacterData(Health, Shield, Energy, Mana, Mana);
        }
        
        private bool _isDeath
        {
            get;
            set;
        }

        private event DieDelegate _dieEvent;
        
        public int Health => health;
        public int Shield => shield;
        public int Energy => energy;
        public int Damage => damage;
        public int Mana => mana;

        public DieDelegate DieEvent
        {
            get
            {
                return _dieEvent;
            }
            set
            {
                _dieEvent = value;
            }
        }
        
        public void UseEnergy()
        {
            if(energy<=0) return;
            energy-=25;
            if (energy <= 0) energy = 0;
        }

        public void UseMana(int value)
        {
            mana = Mathf.Clamp(mana - value, 0, 100);
        }

        public void Damaged(int value)
        {
          //  if(_isDeath) return;
            int dmgToHealt=0;
            dmgToHealt = Mathf.Clamp(value - shield, 0 , int.MaxValue);
            health = Mathf.Clamp(health - dmgToHealt, 0, 1000);
            Die();
        }

        private void Die()
        {
            if (health != 0) return;
            _isDeath = true;
            _dieEvent?.Invoke();
        }
    }
}