using System;
using System.Threading.Tasks;
using UI.CombatHUD;
using UnityEngine;

namespace Characters.Player
{
    public delegate void DieDelegate();
    [Serializable]
    public class CharacterData
    {
        [SerializeField] private float health;
        [SerializeField] private float shield;
        [SerializeField] private float energy;
        [SerializeField] private float mana;
        [SerializeField] protected int damage;
        private float _healthMax;
        private float _energyMax;
        private float _manaMax;
        private event UpdateManaDelegate _updateManaEvent;
        private event UpdateHealthDelegate _updateHealthEvent;
        private event UpdateEnergyDelegate _updateEnergyEvent;

        public CharacterData(float health, float shield, float energy, float mana, int damage)
        {
            this.health = health;
            this.shield = shield;
            this.energy = energy;
            this.mana = mana;
            this.damage = damage;
            _healthMax = health;
            _energyMax = energy;
            _manaMax = mana;
            AddResource();
        }

        private async void AddResource()
        {
            for (int i = 0; !_isDeath;)
            {
                await Task.Delay(1000);
                AddEnergy(1f);
                AddMana(1f);
                AddHealth(1f);
            }
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
        
        public float Health => health;
        public float Shield => shield;
        public float Energy => energy;
        public float Mana => mana;
        public int Damage => damage;

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
            energy= Mathf.Clamp(energy-5,0,_energyMax);
            _updateEnergyEvent?.Invoke(energy);
            if (energy <= 0) energy = 0;
        }

        public void AddEnergy(float value)
        {
            energy = Mathf.Clamp(energy+value,0,_energyMax);
            _updateEnergyEvent?.Invoke(energy);
        }

        public void AddHealth(float value)
        {
            health = Mathf.Clamp(health+value,0,_healthMax);
            _updateHealthEvent?.Invoke(health);
        }

        public void AddMana(float value)
        {
            mana = Mathf.Clamp(mana+value,0,_manaMax);
            _updateManaEvent?.Invoke(mana);
        }
        public void UseMana(float value)
        {
            mana = Mathf.Clamp(mana - value, 0, _manaMax);
            _updateManaEvent?.Invoke(mana);
        }

        public void Damaged(int value)
        {
          //  if(_isDeath) return;
            float dmgToHealt=0;
            dmgToHealt = Mathf.Clamp(value - shield, 0 , int.MaxValue);
            health = Mathf.Clamp(health - dmgToHealt, 0, _healthMax);
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