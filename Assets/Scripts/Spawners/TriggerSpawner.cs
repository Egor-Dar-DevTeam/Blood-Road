using System;
using Characters;
using Characters.Player;
using UI.EnemyesCanvas;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Spawners
{
    public class TriggerSpawner : MonoBehaviour
    {
        [SerializeField] private EnemySpawnInfo[] enemySpawnInfo;
        [SerializeField] private PanelsCreator panelsCreator;
        private bool _continue;

        public void SetPanelsCreator(PanelsCreator panelsCreator)
        {
            this.panelsCreator = panelsCreator;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.TryGetComponent(out IInteractable player)) return;
            if (!player.IsPlayer() || _continue) return;
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
            enemy.SetCharacterData(data, panelsCreator.AddCharacter(enemy.VFXTransforms.Up));
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
    }
}