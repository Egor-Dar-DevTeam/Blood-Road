using System;
using System.Threading.Tasks;
using Characters.WeaponSystem;
using UI;
using UI.CombatHUD;
using UnityEngine;

namespace Characters.Player
{
    public delegate void DieDelegate();

    public delegate void DieInteractable(IInteractable interactable);

    public delegate void Impenetrable(bool value);

    [Serializable]
    public class CharacterData
    {
        [SerializeField] private float health;
        [SerializeField] private float shield;
        [SerializeField] private float energy;
        [SerializeField] private float mana;
        [SerializeField] protected Weapon weapon;
        private int damage ;
        private float _healthMax;
        private float _energyMax;
        private float _manaMax;
        private int _maxDamage => weapon.EffectData.HealthDamage;
        private float _additionalHealthWithDamage = 0;

        private bool _isImpenetrable;
        private IInteractable _currentInteractable;
        private event UpdateManaDelegate _updateManaEvent;
        private event UpdateHealthDelegate _updateHealthEvent;
        private event UpdateEnergyDelegate _updateEnergyEvent;
        public Impenetrable ImpenetrableDelegate;
        public VFXTransforms weaponTransforms => weapon.VFXTransforms;

        public CharacterData(float health, float shield, float energy, float mana, Weapon weapon, IInteractable interactable)
        {
            this.health = health;
            this.shield = shield;
            this.energy = energy;
            this.mana = mana;
            this.weapon = weapon;
            _healthMax = health;
            _energyMax = energy;
            _manaMax = mana;
            damage = _maxDamage;
            ImpenetrableDelegate = Impenetrable;
            _currentInteractable = interactable;
            AddResource();
        }

        public void SetInteractable(IInteractable interactable) => _currentInteractable = interactable;

        private async void AddResource()
        {
            for (; !_isDeath;)
            {
                await Task.Delay(1000);
                AddEnergy(1f);
                AddMana(1f);
                AddHealth(1f);
            }
        }

        public CharacterData Copy()
        {
            return new CharacterData(Health, Shield, Energy, Mana, weapon, _currentInteractable);
        }

        private void Impenetrable(bool value) => _isImpenetrable = value;

        public void EventsInitialize(UIDelegates delegates)
        {
            _updateEnergyEvent += delegates.UpdateEnergyDelegate;
            _updateHealthEvent += delegates.UpdateHealthDelegate;
            _updateManaEvent += delegates.UpdateManaDelegate;
            _dieEvent += () => UnsubscribeUIDelegates(delegates);
            _dieEvent += delegates.DieDelegate;

            _updateManaEvent?.Invoke(mana, _manaMax);
            _updateEnergyEvent?.Invoke(energy, _energyMax);
            _updateHealthEvent?.Invoke(health, _healthMax);
        }

        private void UnsubscribeUIDelegates(UIDelegates delegates)
        {
            _updateEnergyEvent -= delegates.UpdateEnergyDelegate;
            _updateHealthEvent -= delegates.UpdateHealthDelegate;
            _updateManaEvent -= delegates.UpdateManaDelegate;
            _dieEvent -= delegates.DieDelegate;
            _dieInteractable = null;
        }

        private bool _isDeath { get; set; }

        private event DieDelegate _dieEvent;
        private event DieInteractable _dieInteractable;

        public float Health => health;
        public float Shield => shield;
        public float Energy => energy;
        public float Mana => mana;
        public int Damage => damage;

        public DieDelegate DieEvent
        {
            get { return _dieEvent; }
            set { _dieEvent = value; }
        }

        public DieInteractable DieInteractable        {
            get { return _dieInteractable; }
            set { _dieInteractable = value; }
        }

        public void SetAdditionalHealthAfterDamage(bool value)
        {
            _additionalHealthWithDamage = value ? damage / 4 : 0;
        }

        public void IncreaseDamageIn(int value)
        {
            damage = value == 1 ? _maxDamage : damage * value;
        }

        public void UseEnergy()
        {
            if (energy <= 0) return;
            energy = Mathf.Clamp(energy - 5, 0, _energyMax);
            _updateEnergyEvent?.Invoke(energy, _energyMax);
            AddHealth(_additionalHealthWithDamage);
            if (energy <= 0) energy = 0;
        }

        public void AddEnergy(float value)
        {
            energy = Mathf.Clamp(energy + value, 0, _energyMax);
            _updateEnergyEvent?.Invoke(energy, _energyMax);
        }

        public void AddHealth(float value)
        {
            health = Mathf.Clamp(health + value, 0, _healthMax);
            _updateHealthEvent?.Invoke(health, _healthMax);
        }

        public void AddMana(float value)
        {
            mana = Mathf.Clamp(mana + value, 0, _manaMax);
            _updateManaEvent?.Invoke(mana, _manaMax);
        }

        public void UseMana(float value)
        {
            mana = Mathf.Clamp(mana - value, 0, _manaMax);
            _updateManaEvent?.Invoke(mana, _manaMax);
        }

        public void Damaged(int value)
        {
            if (_isImpenetrable) return;
            float dmgToHealt = 0;
            dmgToHealt = Mathf.Clamp(value - shield, 0, int.MaxValue);
            health = Mathf.Clamp(health - dmgToHealt, 0, _healthMax);
            _updateHealthEvent?.Invoke(health, _healthMax);
            Die();
        }

        private void Die()
        {
            if (health != 0) return;
            _isDeath = true;
            _dieInteractable?.Invoke(_currentInteractable);
            _dieEvent?.Invoke();
        }
    }
}