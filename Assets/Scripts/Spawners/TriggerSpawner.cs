using System;
using System.Collections.Generic;
using Characters;
using Characters.Player;
using MapSystem;
using UI.EnemyesCanvas;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Spawners
{
    public class TriggerSpawner : MonoBehaviour
    {
        [SerializeField] private List<EnemySpawnInfo> enemySpawnInfo;
        [SerializeField] private PanelsCreator panelsCreator;
        [SerializeField] private EnemiesData enemiesData;
        [SerializeField] private Placeholder placeholder;
        private bool _continue;
        public EnemiesData EnemiesData => enemiesData;

        public void SetPanelsCreator(PanelsCreator panelsCreator)
        {
            this.panelsCreator = panelsCreator;
        }
        public void AddPointAndEnemy(Transform point, EnemyData enemyData)
        {
            enemySpawnInfo.Add(new EnemySpawnInfo(enemyData, point));
        }
        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.TryGetComponent(out IInteractable player)) return;
            if (!player.IsPlayer() || _continue) return;
            foreach (var info in enemySpawnInfo)
            {
                Instantiate(info.EnemyPrefab, info.EnemyData, info.Position);
            }

            _continue = true;
        }

        private void Instantiate(BaseCharacter prefab, CharacterData data, Vector3 position)
        {
            var enemy = Object.Instantiate(prefab, position, Quaternion.identity);
            enemy.SetPlaceholder(placeholder);
            data.SetInteractable(enemy);
            enemy.SetCharacterData(data);
            panelsCreator.AddCharacter(enemy);
        }
    }

    [Serializable]
    public struct EnemySpawnInfo
    {
        [SerializeField] private EnemyData data;
        [SerializeField] private Transform point;
        public BaseCharacter EnemyPrefab => data.Character;
        public CharacterData EnemyData => data.Data;
        public Vector3 Position => point.position;

        public EnemySpawnInfo(EnemyData enemyData, Transform point)
        {
            data = enemyData;
            this.point = point;
        }
    }
}