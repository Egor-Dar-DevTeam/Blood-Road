using Characters.AbilitiesSystem;
using Cinemachine;
using Spawners;
using UI.EnemyesCanvas;
using UnityEngine;

namespace Scripts
{
    public class LevelGenerator : MonoBehaviour
    {
        public CameraController cameraController;
        public PanelsCreator panelsCreator;
        public Transform startPoint;
        public Transform endPoint;
        public AbilityTrigger triggerPrefab;
        public TriggerSpawner spawnerPrefab;
        public int n;
    }
}