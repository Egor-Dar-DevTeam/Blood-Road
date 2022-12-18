using System.Collections.Generic;
using System.Linq;
using Characters.Facades;
using Characters.InteractableSystems;
using Characters.Player;
using UnityEngine;
using UnityEngine.AI;

namespace Characters.Enemy
{
    public class DefaultEnemy : MonoBehaviour, IInteractable
    {
        [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private Animator animator;
        [SerializeField] private Eyes eyesCharacters;
        [SerializeField] private float rotationSpeed=1f;
        private IInteractable _currentPoint;
        private SetCurrentPoint _setCurrentPoint;
        private StartRechangeCurrentPoint _startRechangeCurrentPoint;
        private GetCurrentPoint _getCurrentPoint;
        
        private InteractionSystem _interactionSystem;
        private TransitionAndStates _transitionAndStates;
        private IInteractable GetCurrentPoint() => _currentPoint;
        
        private void Start()
        {
            _setCurrentPoint = SetCurrentPoint;
            _startRechangeCurrentPoint = StartRCP;
            _getCurrentPoint = GetCurrentPoint;

            _transitionAndStates = new EnemyTransition();
            _transitionAndStates.Initialize(animator, _getCurrentPoint, transform, agent, null, null);

            _interactionSystem = new InteractionSystem();
            _interactionSystem.Initialize(null, eyesCharacters, transform, _setCurrentPoint,
                _startRechangeCurrentPoint);
        }
        private void Update()
        {
            _transitionAndStates.Update();

            if (_currentPoint != null)
            {
                var turnTowardNavSteeringTarget = agent.steeringTarget;

                Vector3 direction = (turnTowardNavSteeringTarget - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
            }
        }

        private void StartRCP(List<IInteractable> points)
        {
            foreach (var point in points.Where(point => point.IsPlayer()))
            {
                SetCurrentPoint(point);
            }
        }

        private void SetCurrentPoint(IInteractable point)
        {
            _currentPoint = point;
        }

        public void SetOutline(Material outline)
        {
            skinnedMeshRenderer.material = outline;
        }

        public Transform GetObject() => this.transform;
        public bool IsPlayer()=>false;
    }
}
