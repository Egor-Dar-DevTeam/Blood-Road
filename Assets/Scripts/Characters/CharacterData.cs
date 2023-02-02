using System;
using UI.CombatHUD;
using UnityEngine;

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
        private event UpdateManaDelegate _updateManaEvent;
        private event UpdateHealthDelegate _updateHealthEvent;
        private event UpdateEnergyDelegate _updateEnergyEvent;

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
            return new CharacterData(Health, Shield, Energy, Mana, Damage);
        }

        public void EventsInitialize(UpdateManaDelegate updateManaDelegate, UpdateHealthDelegate updateHealthDelegate,
            UpdateEnergyDelegate updateEnergyDelegate)
        {
            _updateEnergyEvent += updateEnergyDelegate;
            _updateHealthEvent += updateHealthDelegate;
            _updateManaEvent += updateManaDelegate;
            _updateManaEvent.Invoke(mana);
            _updateEnergyEvent.Invoke(energy);
            _updateHealthEvent.Invoke(health);
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
            _updateEnergyEvent?.Invoke(energy);
            if (energy <= 0) energy = 0;
        }

        public void AddEnergy(int value)
        {
            energy += value;
            _updateEnergyEvent?.Invoke(energy);
        }

        public void AddHealth(int value)
        {
            health += value;
            _updateHealthEvent?.Invoke(health);
        }

        public void AddMana(int value)
        {
            mana += value;
            _updateManaEvent?.Invoke(mana);
        }
        public void UseMana(int value)
        {
            mana = Mathf.Clamp(mana - value, 0, 100);
            _updateManaEvent?.Invoke(mana);
        }

        public void Damaged(int value)
        {
          //  if(_isDeath) return;
            int dmgToHealt=0;
            dmgToHealt = Mathf.Clamp(value - shield, 0 , int.MaxValue);
            health = Mathf.Clamp(health - dmgToHealt, 0, 1000);
            _updateHealthEvent?.Invoke(health);
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