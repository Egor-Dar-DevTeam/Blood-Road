using System;
using System.Threading.Tasks;
using UI;
using UI.CombatHUD;
using UnityEngine;

namespace Characters.Player
{
    public delegate void DieDelegate();
    public delegate void Impenetrable(bool value);
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

        private bool _isImpenetrable;
        private event UpdateManaDelegate _updateManaEvent;
        private event UpdateHealthDelegate _updateHealthEvent;
        private event UpdateEnergyDelegate _updateEnergyEvent;
        public Impenetrable ImpenetrableDelegate;

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
            ImpenetrableDelegate = Impenetrable;
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

        private void Impenetrable(bool value) => _isImpenetrable = value;

        public void EventsInitialize(UIDelegates delegates)
        {
            _updateEnergyEvent += delegates.UpdateEnergyDelegate;
            _updateHealthEvent += delegates.UpdateHealthDelegate;
            _updateManaEvent += delegates.UpdateManaDelegate;
            _dieEvent += delegates.DieDelegate;
            
            _updateManaEvent?.Invoke(mana, _manaMax);
            _updateEnergyEvent?.Invoke(energy, _energyMax);
            _updateHealthEvent?.Invoke(health, _healthMax);
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
            _updateEnergyEvent?.Invoke(energy, _energyMax);
            if (energy <= 0) energy = 0;
        }

        public void AddEnergy(float value)
        {
            energy = Mathf.Clamp(energy+value,0,_energyMax);
            _updateEnergyEvent?.Invoke(energy, _energyMax);
        }

        public void AddHealth(float value)
        {
            health = Mathf.Clamp(health+value,0,_healthMax);
            _updateHealthEvent?.Invoke(health, _healthMax);
        }

        public void AddMana(float value)
        {
            mana = Mathf.Clamp(mana+value,0,_manaMax);
            _updateManaEvent?.Invoke(mana, _manaMax);
        }
        public void UseMana(float value)
        {
            mana = Mathf.Clamp(mana - value, 0, _manaMax);
            _updateManaEvent?.Invoke(mana, _manaMax);
        }

        public void Damaged(int value)
        {
            if(_isImpenetrable) return;
            float dmgToHealt=0;
            dmgToHealt = Mathf.Clamp(value - shield, 0 , int.MaxValue);
            health = Mathf.Clamp(health - dmgToHealt, 0, _healthMax);
            _updateHealthEvent?.Invoke(health, _healthMax);
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