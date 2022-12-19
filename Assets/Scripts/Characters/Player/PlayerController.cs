using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Characters.Facades;
using Characters.InteractableSystems;
using Characters.Player.States;
using UI;
using UI.CombatHUD;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

namespace Characters.Player
{
    public delegate bool GetIsAttack();

    public delegate void StartRechangeCurrentPoint(List<IInteractable> points);

    public delegate IInteractable GetCurrentPoint();


    public class PlayerController : MonoBehaviour, IInteractable
    {
        [SerializeField] private float rotationSpeed;
        [SerializeField] private Material outlineMaterial;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private Animator animator;
        [SerializeField] private Eyes eyesCharacters;
        [SerializeField] private CameraRay cameraRay;
        [SerializeField] private PlayerData playerData;
        [SerializeField] private GameCanvasController canvasController;
        [SerializeField] private AnimationClip trackClip;

        private TASData _tASData;

        private event UpdateEnergyDelegate _updateEnergyEvent;
        private event UpdateHealthDelegate _updateHealthEvent;
        private event UpdateShieldDelegate _updateShieldEvent;

        #region Intefaces

        private IInteractable _currentPoint;

        #endregion


        #region Delegates

        private GetIsAttack _getIsAttack;
        private SetCurrentPoint _setCurrentPoint;
        private StartRechangeCurrentPoint _startRechangeCurrentPoint;
        private GetCurrentPoint _getCurrentPoint;

        #endregion

        #region ClassesNotSerializables

        private EnemyOutlineRechanger _enemyOutlineRechanger;
        private InteractionSystem _interactionSystem;
        private TransitionAndStates _transitionAndStates;

        #endregion

        private bool isAttack;
        private bool GetIsAttack() => isAttack;
        private PlayerData GetPlayerData() => playerData;
        private IInteractable GetCurrentPoint() => _currentPoint;
        public Transform GetObject() => transform;

        public bool IsPlayer() => true;

        private void Start()
        {
            var updateEnergyDelegate = canvasController.UIDelegates.UpdateEnergyDelegate;
            _updateEnergyEvent = updateEnergyDelegate;
            updateEnergyDelegate += _updateEnergyEvent;

            var updateHealthDelegate = canvasController.UIDelegates.UpdateHealthDelegate;
            _updateHealthEvent = updateHealthDelegate;
            updateHealthDelegate += _updateHealthEvent;

            var updateShieldDelegate = canvasController.UIDelegates.UpdateShieldDelegate;
            _updateShieldEvent = updateShieldDelegate;
            updateShieldDelegate += _updateShieldEvent;


            _enemyOutlineRechanger = new EnemyOutlineRechanger(outlineMaterial);
            _getIsAttack = GetIsAttack;
            _setCurrentPoint = SetCurrentPoint;
            _startRechangeCurrentPoint = StartRCP;
            _getCurrentPoint = GetCurrentPoint;

            _tASData = new TASData(animator, _getCurrentPoint, transform,
                agent, _getIsAttack, playerData,trackClip);

            _transitionAndStates = new PlayerTransition();
            _transitionAndStates.Initialize(_tASData);

            _interactionSystem = new InteractionSystem();
            _interactionSystem.Initialize(cameraRay, eyesCharacters, transform, _setCurrentPoint,
                _startRechangeCurrentPoint);
        }

        private void Update()
        {
            _transitionAndStates.Update();

            if (_currentPoint != null)
            {
                var turnTowardNavSteeringTarget = _currentPoint.GetObject().transform.position;

                Vector3 direction = (turnTowardNavSteeringTarget - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
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
                if (!points[i].IsPlayer())
                {
                    indx = i;
                    for (int j = 0; j < points.Count; j++)
                    {
                        if (Vector3.Distance(transform.position, points[j].GetObject().position) <=
                            Vector3.Distance(transform.position, points[indx].GetObject().position))
                        {
                            indx = j;
                        }
                    }

                    point = points[indx];
                }
            }

            if (point != null)
            {
                _currentPoint = point;
                _enemyOutlineRechanger.SetEnemy(_currentPoint);
            }

            yield return new WaitForSeconds(0);
        }

        private void SetCurrentPoint(IInteractable point)
        {
            if (point.IsPlayer()) return;
            if (_currentPoint != point)
            {
                _currentPoint = point;
                _enemyOutlineRechanger.SetEnemy(_currentPoint);
            }

            if (playerData.Energy > 1)
            {
              Attack();
            }
        }

        private async Task Attack()
        {
            isAttack = true;
            await Task.Delay(100);
            playerData.UseEnergy();
            _updateEnergyEvent?.Invoke(playerData.Energy);
            await Task.Delay(100);
            isAttack = false;
        }

        public void ReceiveDamage(float value)
        {
            playerData.Damage(value);
            _updateHealthEvent?.Invoke(playerData.Health);
            _updateShieldEvent?.Invoke(playerData.Shield);
            if(playerData.Health==0) Debug.LogWarning("PlayerDeath");
            
        }

        public void SetOutline(Material outline)
        {
        }
    }
}