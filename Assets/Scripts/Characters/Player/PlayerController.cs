using System.Collections;
using System.Collections.Generic;
using Characters.Facades;
using UnityEngine;
using UnityEngine.AI;

namespace Characters.Player
{
    public delegate bool GetIsAttack();

    public delegate void StartRechangeCurrentPoint(List<IInteractable> points);

    public delegate IInteractable GetCurrentPoint();

    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Material outlineMaterial;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private Animator animator;
        [SerializeField] private Eyes eyesCharacters;
        [SerializeField] private CameraRay cameraRay;
        private IInteractable _currentPoint;
        private GetIsAttack _getIsAttack;
        private SetCurrentPoint _setCurrentPoint;
        private StartRechangeCurrentPoint _startRechangeCurrentPoint;
        private GetCurrentPoint _getCurrentPoint;

        private EnemyOutlineRechanger _enemyOutlineRechanger;
        private InteractionSystem _interactionSystem;
        private TransitionAndStates _transitionAndStates;
        private bool isAttack;
        private bool GetIsAttack() => isAttack;
        private IInteractable GetCurrentPoint() => _currentPoint;

        private void Start()
        {
            _enemyOutlineRechanger = new EnemyOutlineRechanger(outlineMaterial);
            _getIsAttack = GetIsAttack;
            _setCurrentPoint = SetCurrentPoint;
            _startRechangeCurrentPoint = StartRCP;
            _getCurrentPoint = GetCurrentPoint;

            _transitionAndStates = new TransitionAndStates();
            _transitionAndStates.Initialize(animator, _getCurrentPoint, transform, agent, _getIsAttack);

            _interactionSystem = new InteractionSystem();
            _interactionSystem.Initialize(cameraRay, eyesCharacters, transform, _setCurrentPoint,
                _startRechangeCurrentPoint);
        }

        private void Update()
        {
            _transitionAndStates.Update();
            if (isAttack)
            {
                if (_currentPoint == null)
                    isAttack = false;
            }
            else
            {
                if (_currentPoint != null)
                    if (Vector3.Distance(transform.position, _currentPoint.GetObject().position) <=
                        agent.stoppingDistance)
                        isAttack = true;
            }
        }

        private void StartRCP(List<IInteractable> points)
        {
            StartCoroutine(RechangeCurrentPoint(points));
        }

        private IEnumerator RechangeCurrentPoint(List<IInteractable> points)
        {
            if (_currentPoint != null) yield break;
            int indx;
            IInteractable point = null;
            for (int i = 0; i < points.Count; i++)
            {
                indx = i;
                for (int j = 0; j < points.Count; j++)
                {
                    if (Vector3.Distance(transform.position, points[j].GetObject().position) <
                        Vector3.Distance(transform.position, points[indx].GetObject().position))
                    {
                        indx = j;
                    }
                }

                if (Vector3.Distance(transform.position, points[indx].GetObject().position) ==
                    Vector3.Distance(transform.position,
                        points[i].GetObject().position))
                    continue;

                point = points[indx];
            }

            SetCurrentPoint(point);

            yield return new WaitForSeconds(0);
        }

        private void SetCurrentPoint(IInteractable point)
        {
            _currentPoint = point;
            _enemyOutlineRechanger.SetEnemy(_currentPoint);
        }
    }
}