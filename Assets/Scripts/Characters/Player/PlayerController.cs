using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Characters.Facades;
using UI;
using UI.CombatHUD;
using UnityEngine;

namespace Characters.Player
{
    public class PlayerController : BaseCharacter, IInteractable
    {
        [SerializeField] private Material outlineMaterial;
        [SerializeField] private CameraRay cameraRay;
        [SerializeField] private GameCanvasController canvasController;

        private List<IInteractable> _interactables;

        private event UpdateEnergyDelegate _updateEnergyEvent;
        private event UpdateHealthDelegate _updateHealthEvent;


        #region Delegates

        private GetIsAttack _getIsAttack;

        #endregion

        #region ClassesNotSerializables

        private EnemyOutlineRechanger _enemyOutlineRechanger;

        #endregion

        private bool isAttack;
        private bool GetIsAttack() => isAttack;
        public override bool IsPlayer() => true;

        protected override void Start()
        {
            _interactables = new List<IInteractable>();

            _updateEnergyEvent += canvasController.UIDelegates.UpdateEnergyDelegate;

            _updateHealthEvent += canvasController.UIDelegates.UpdateHealthDelegate;
            
            _enemyOutlineRechanger = new EnemyOutlineRechanger(outlineMaterial);
            _getIsAttack = GetIsAttack;
            base.Start();
            InitializeTransition(new PlayerTransition(), _getIsAttack, _updateEnergyEvent);
            InitializeInteractionSystem(cameraRay);
            SubscribeDeath();
        }


        protected override void ClearPoint()
        {
            characterData.DieEvent -= _currentPoint.GetDieCharacterDelegate();
            _interactables.Remove(_currentPoint);
            _currentPoint = null;
            _enemyOutlineRechanger.SetEnemy(null);
            StartCoroutine(RechangeCurrentPoint());
        }

        protected override void StartRCP(List<IInteractable> points)
        {
            AddToListEnemy(points);
        }

        private void AddToListEnemy(List<IInteractable> enemies)
        {
            foreach (var enemy in enemies)
            {
                if (!enemy.HasCharacter()) continue;
                if (_interactables.Contains(enemy)) continue;
                characterData.DieEvent += enemy.GetDieCharacterDelegate();
                _interactables.Add(enemy);
            }

            StartCoroutine(RechangeCurrentPoint());
        }

        private IEnumerator RechangeCurrentPoint()
        {
            if (_currentPoint == null)
            {
                if (_interactables.Count == 0) yield break;
                int indx;
                IInteractable point = null;
                for (int i = 0; i < _interactables.Count; i++)
                {
                    if (!_interactables[i].IsPlayer())
                    {
                        indx = i;
                        for (int j = 0; j < _interactables.Count; j++)
                        {
                            if (Vector3.Distance(transform.position, _interactables[j].GetObject().position) <=
                                Vector3.Distance(transform.position, _interactables[indx].GetObject().position))
                            {
                                indx = j;
                            }
                        }

                        point = _interactables[indx];
                    }
                }

                if (point != null && point.HasCharacter())
                {
                    _currentPoint = point;
                    _enemyOutlineRechanger.SetEnemy(_currentPoint);
                }

                yield return new WaitForSeconds(0);
            }
        }

        protected override void SetCurrentPoint(IInteractable point)
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
            await Task.Delay(200);
            isAttack = false;
        }

        public override void ReceiveDamage(int value)
        {
            characterData.Damaged(value);
            _updateHealthEvent?.Invoke(characterData.Health);
        }

        public override void SetOutline(Material outline)
        {
        }
    }
}