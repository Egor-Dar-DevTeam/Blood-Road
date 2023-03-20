using System;
using System.Collections.Generic;
using System.Linq;
using Spawners;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Editor
{
    [CustomEditor(typeof(TriggerSpawner))]
    public class TriggerSpawnerEditor : UnityEditor.Editor
    {
        private TriggerSpawner _triggerSpawner;
        private List<EnemyData> _enemiesData;

        private void OnEnable()
        {
            _triggerSpawner = target as TriggerSpawner;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if(GUILayout.Button("Set Points")) SetPoints();
        }

        private void SetPoints()
        {
            _enemiesData = _triggerSpawner.EnemiesData.GetList;
            foreach (var childTransform in _triggerSpawner.GetComponentsInChildren<Transform>().ToList())
            {
                if(childTransform.name =="Spawner (Clone)") return;
                var enemy = _enemiesData[Random.Range(0, _enemiesData.Count)];
                _enemiesData.Remove(enemy);
                _triggerSpawner.AddPointAndEnemy(childTransform, enemy);
            }
        }
    }
}
