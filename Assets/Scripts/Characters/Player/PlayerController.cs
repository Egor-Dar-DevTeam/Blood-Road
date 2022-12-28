using System;
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
using UnityEngine.Serialization;

namespace Characters.Player
{
    public delegate bool GetIsAttack();

    public delegate void StartRechangeCurrentPoint(List<IInteractable> points);

    public delegate IInteractable GetCurrentPoint();

    public delegate bool HasCharacter();


    public class PlayerController : MonoBehaviour, IInteractable
    {
        [SerializeField] private float rotationSpeed;
        [SerializeField] private Material outlineMaterial;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private Animator animator;
        [SerializeField] private Eyes eyesCharacters;
        [SerializeField] private CameraRay cameraRay;
        [SerializeField] private CharacterData characterData;
        [SerializeField] private GameCanvasController canvasController;
        [SerializeField] private AnimationClip trackClip;
        [SerializeField] private CapsuleCollider capsuleCollider;
        private bool _hasCharacter = true;

        private TASData _tASData;
        private List<IInteractable> _interactables;

        private event UpdateEnergyDelegate _updateEnergyEvent;
        private event UpdateHealthDelegate _updateHealthEvent;

        #region Intefaces

        private IInteractable _currentPoint;

        #endregion


        #region Delegates

        private GetIsAttack _getIsAttack;
        private SetCurrentPoint _setCurrentPoint;
        private StartRechangeCurrentPoint _startRechangeCurrentPoint;
        private GetCurrentPoint _getCurrentPoint;
        private DieDelegate _characterPointDie;
        private HasCharacter _hasCharacterDelegate;

        #endregion

        #region ClassesNotSerializables

        private EnemyOutlineRechanger _enemyOutlineRechanger;
        private InteractionSystem _interactionSystem;
        private TransitionAndStates _transitionAndStates;

        #endregion

        private bool isAttack;
        private bool GetIsAttack() => isAttack;
        private IInteractable GetCurrentPoint() => _currentPoint;
        public Transform GetObject() => transform;
        public bool IsPlayer() => true;
        public DieDelegate GetDieCharacterDelegate() => _characterPointDie+characterData.DieEvent;
        public event DieDelegate GetDieEvent;
        public bool HasCharacter() => _hasCharacter;

        private void Start()
        {
            _interactables = new List<IInteractable>();
            
            _updateEnergyEvent += canvasController.UIDelegates.UpdateEnergyDelegate;

            _updateHealthEvent += canvasController.UIDelegates.UpdateHealthDelegate;

            _hasCharacterDelegate = HasCharacter;

            _enemyOutlineRechanger = new EnemyOutlineRechanger(outlineMaterial);
            _getIsAttack = GetIsAttack;
            _setCurrentPoint = SetCurrentPoint;
            _startRechangeCurrentPoint = StartRCP;
            _getCurrentPoint = GetCurrentPoint;

            _tASData = new TASData(animator, _getCurrentPoint, transform,
                agent, _getIsAttack, characterData, trackClip, null, characterData.Damage, _hasCharacterDelegate,
                capsuleCollider);

            _transitionAndStates = new PlayerTransition();
            _transitionAndStates.Initialize(_tASData);

            _interactionSystem = new InteractionSystem();
            _interactionSystem.Initialize(cameraRay, eyesCharacters, transform, _setCurrentPoint,
                _startRechangeCurrentPoint);

            characterData.DieEvent+=_transitionAndStates.DieDelegate;
            characterData.DieEvent += () => _hasCharacter = false;

            _characterPointDie = ClearPoint;
            GetDieEvent = characterData.DieEvent;
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


        private  void ClearPoint()
        {
            characterData.DieEvent -= _currentPoint.GetDieCharacterDelegate();
            _interactables.Remove(_currentPoint);
            _currentPoint = null;
            _enemyOutlineRechanger.SetEnemy(null);
            StartCoroutine( RechangeCurrentPoint(_interactables));
        }

        private void StartRCP(List<IInteractable> points)
        {
            AddToListEnemy(points);
        }

        private void AddToListEnemy(List<IInteractable> enemies)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if (!_interactables.Contains(enemies[i]))
                {
                    characterData.DieEvent += enemies[i].GetDieCharacterDelegate();
                    _interactables.Add(enemies[i]);
                }
            }
            StartCoroutine(RechangeCurrentPoint(_interactables));

        }

        private IEnumerator RechangeCurrentPoint(List<IInteractable> points)
        {
            if (_currentPoint == null)
            {
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

                if (point != null&& point.HasCharacter())
                {
                    _currentPoint = point;
                    _enemyOutlineRechanger.SetEnemy(_currentPoint);
                }

                yield return new WaitForSeconds(0);
            }
        }

        private void SetCurrentPoint(IInteractable point)
        {
            if (point.IsPlayer()) return;
            if (_currentPoint != point)
            {
                _currentPoint = point;
                _enemyOutlineRechanger.SetEnemy(_currentPoint);
            }

            if (characterData.Energy > 1)
            {
                Attack();
            }
        }

        private async Task Attack()
        {
            isAttack = true;
            await Task.Delay(100);
            characterData.UseEnergy();
            _updateEnergyEvent?.Invoke(characterData.Energy);
            await Task.Delay(100);
            isAttack = false;
        }

        public void ReceiveDamage(int value)
        {
            characterData.Damaged(value);
            _updateHealthEvent?.Invoke(characterData.Health);
        }

        public void SetOutline(Material outline)
        {
        }

        private void OnDestroy()
        {
            _hasCharacter = false;
            _transitionAndStates.Destroy();
        }
    }
}