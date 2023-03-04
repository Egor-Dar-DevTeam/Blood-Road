using Characters.AbilitiesSystem;
using Cinemachine;
using Dreamteck.Splines;
using Spawners;
using UI.EnemyesCanvas;
using UnityEngine;
using UnityEngine.Serialization;

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