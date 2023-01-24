using Characters;
using Characters.Player;
using UnityEngine;

namespace Spawners
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Enemy", fileName = "Enemy", order = 1)]
    public class EnemyData : ScriptableObject
    {
        [SerializeField] private BaseCharacter character;
        [SerializeField] private CharacterData data;

        public BaseCharacter Character => character;
        public CharacterData Data => data;
    }
}