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
        [SerializeField] protected int damage;
        [SerializeField] private VFXEffect damageEffect;
        [SerializeField] private Transform transform;
        private bool _isDeath;

        private event DieDelegate _dieEvent;
        
        public int Health => health;
        public int Energy => energy;
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
            energy-=25;
            if (energy <= 0) energy = 0;
        }

        public void Damaged(int value)
        {
            if(_isDeath) return;
            int dmgToHealt=0;
            Object.Instantiate(damageEffect, transform.position, quaternion.identity);
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