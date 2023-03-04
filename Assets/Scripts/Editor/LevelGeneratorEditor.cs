using System;
using Characters.AbilitiesSystem;
using Cinemachine;
using Dreamteck.Splines;
using Scripts;
using Spawners;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(LevelGenerator))]
    public class LevelGeneratorEditor : UnityEditor.Editor
    {
        private LevelGenerator _levelGenerator;

        private void OnEnable()
        {
            _levelGenerator = target as LevelGenerator;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Generate")) OnGenerate();
        }

        private void OnSceneGUI()
        {
            if (_levelGenerator.startPoint == null || _levelGenerator.endPoint == null) return;
            Handles.DrawLine(_levelGenerator.startPoint.position, _levelGenerator.endPoint.position, 1);
        }

        private void OnGenerate()
        {
            var dist = _levelGenerator.startPoint.position.z - _levelGenerator.endPoint.position.z;
            dist = Math.Abs(dist) / _levelGenerator.n;
            var parentTrigger = new GameObject
            {
                name = "TriggerParent"
            };
            var parentSpawner = new GameObject
            {
                name = "SpawnerParent"
            };
            for (int i = 0; i < _levelGenerator.n; i++)
            {
                var position = _levelGenerator.startPoint.position;
                var triggerPosition = new Vector3(0, 0, position.z + (dist * i));
                var trigger = Instantiate(_levelGenerator.triggerPrefab, parentTrigger.transform, true);
                trigger.transform.position = triggerPosition;
                trigger.SetCameraController(_levelGenerator.cameraController);


                var spawnerPosition = Vector3.Lerp(new Vector3(0, 0, position.z + (dist * i)),
                    new Vector3(0, 0, position.z + (dist * (i + 1))), 0.5f);
                var spawner = Instantiate(_levelGenerator.spawnerPrefab, parentSpawner.transform, true);
                spawner.transform.position = spawnerPosition;
                spawner.SetPanelsCreator(_levelGenerator.panelsCreator);
            }
        }
    }
}