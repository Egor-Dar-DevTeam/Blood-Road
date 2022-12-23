using System;
using Random = UnityEngine.Random;

namespace Characters.Player
{
    [Serializable]
    public class EnemyData : CharacterData
    {
        public int Damage => Random.Range(damage - 10, damage);
    }
}