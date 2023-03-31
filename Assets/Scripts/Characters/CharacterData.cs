using System;
using System.Threading.Tasks;
using Characters.WeaponSystem;
using UnityEngine;

namespace Characters.Player
{
    public delegate void DieInteractable(IInteractable interactable);

    public delegate void Impenetrable(bool value);

    public delegate bool GetRecoil();

    [Serializable]
    public class CharacterData : ICharacterDataSubscriber
    {
        [SerializeField] private float health;
        [SerializeField] private float shield;
        [SerializeField] private float energy;
        [SerializeField] private float mana;
        [SerializeField] protected Weapon weapon;
        private int damage;
        private float _healthMax;
        private float _energyMax;
        private float _manaMax;
        private int _maxDamage => weapon.EffectData.HealthDamage;
        private float _additionalHealthWithDamage = 0;
        private bool _recoil;

        private bool _isImpenetrable;
        private IInteractable _currentInteractable;
        public event Action<float, float> ManaEvent;
        public event Action<float, float> HealthEvent;
        public event Action<float, float> EnergyEvent;
        public event Action DieEvent;

        public Impenetrable ImpenetrableDelegate;
        public GetRecoil GetRecoilDelegate;
        public VFXTransforms weaponTransforms => weapon.VFXTransforms;

        public CharacterData(float health, float shield, float energy, float mana, Weapon weapon,
            IInteractable interactable)
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
            GetRecoilDelegate = GetRecoil;
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

        private bool _isDeath { get; set; }

        private event DieInteractable _dieInteractable;

        public float Health => health;
        public float Shield => shield;
        public float Energy => energy;
        public float Mana => mana;
        public int Damage => damage;

        public DieInteractable DieInteractable
        {
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
            EnergyEvent?.Invoke(energy, _energyMax);
            AddHealth(_additionalHealthWithDamage);
            if (energy <= 0) energy = 0;
        }

        public void AddEnergy(float value)
        {
            if (_isDeath) return;
            energy = Mathf.Clamp(energy + value, 0, _energyMax);
            EnergyEvent?.Invoke(energy, _energyMax);
        }

        public void AddHealth(float value)
        {
            if (_isDeath) return;

            health = Mathf.Clamp(health + value, 0, _healthMax);
            HealthEvent?.Invoke(health, _healthMax);
        }

        public void AddMana(float value)
        {
            if (_isDeath) return;

            mana = Mathf.Clamp(mana + value, 0, _manaMax);
            ManaEvent?.Invoke(mana, _manaMax);
        }

        public void UseMana(float value)
        {
            if (_isDeath) return;

            mana = Mathf.Clamp(mana - value, 0, _manaMax);
            ManaEvent?.Invoke(mana, _manaMax);
        }

        public void Damaged(int value)
        {
            if (_isImpenetrable || _isDeath) return;
            float dmgToHealt = 0;
            dmgToHealt = Mathf.Clamp(value - shield, 0, int.MaxValue);
            health = Mathf.Clamp(health - dmgToHealt, 0, _healthMax);
            HealthEvent?.Invoke(health, _healthMax);
            Die();
        }

        public void DoRecoil()
        {
            _recoil = true;
        }

        private bool GetRecoil()
        {
            if (_recoil)
            {
                _recoil = false;
                return true;
            }

            return false;
        }

        private void Die()
        {
            if (health != 0) return;
            _isDeath = true;
            _dieInteractable?.Invoke(_currentInteractable);
            DieEvent?.Invoke();
            HealthEvent = null;
            EnergyEvent = null;
            ManaEvent = null;
            DieEvent = null;
            _dieInteractable = null;
        }
    }

    public interface ICharacterDataSubscriber
    {
        public event Action<float, float> ManaEvent;
        public event Action<float, float> HealthEvent;
        public event Action<float, float> EnergyEvent;
        public event Action DieEvent;
    }
}