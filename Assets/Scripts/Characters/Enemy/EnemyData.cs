using System;
using Random = UnityEngine.Random;

namespace Characters.Player
{
    [Serializable]
    public class EnemyData : CharacterData
    {
        public int Damage => Random.Range(damage - 10, damage);

        public EnemyData(int health, int shield, int energy, int mana, int damage) : base(health, shield, energy, mana, damage)
        {
        }
    }
}