using System;
using Characters;
using Characters.Player;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Spawners
{
    public class TriggerSpawner : MonoBehaviour
    {
        [SerializeField] private EnemySpawnInfo[] enemySpawnInfo;
        private bool _continue;
        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.TryGetComponent(out IInteractable player)) return;
            if (!player.IsPlayer()|| _continue) return;
            for (int i = 0; i < enemySpawnInfo.Length; i++)
            {
                var info = enemySpawnInfo[i];
                Instantiate(info.EnemyPrefab, info.EnemyData, info.Position);
            }

            _continue = true;
        }

        private void Instantiate(BaseCharacter prefab, CharacterData data, Vector3 position)
        {
            var enemy = Object.Instantiate(prefab, position, Quaternion.identity);
            enemy.SetCharacterData(data);
        }
    }

    [Serializable]
    public struct EnemySpawnInfo
    {
        [SerializeField] private EnemyData data;
        [SerializeField] private Vector3 position;
        public BaseCharacter EnemyPrefab => data.Character;
        public CharacterData EnemyData => data.Data;
        public Vector3 Position => position;
    }
}