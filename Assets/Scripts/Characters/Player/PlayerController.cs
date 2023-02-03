using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Characters.AbilitiesSystem;
using Characters.BottlesSystem;
using Characters.EffectSystem;
using Characters.Facades;
using Dreamteck.Splines;
using UI;
using UnityEngine;

namespace Characters.Player
{
    public delegate void RemoveList(IInteractable interactable);

    public class PlayerController : BaseCharacter, IInteractable
    {
        [SerializeField] private CameraRay cameraRay;
        [SerializeField] private GameCanvasController canvasController;
        [SerializeField] private SplineFollower splineFollower;
        [SerializeField] private SplineProjector projector;

        private List<IInteractable> _interactables;

        public event BottleUse BottleUseEvent;
        

        #region Delegates

        private GetIsAttack _getIsAttack;

        #endregion

        #region ClassesNotSerializables

        private EnemyOutlineRechanger _enemyOutlineRechanger;

        #endregion

        private bool _isAttack;
        private bool GetIsAttack() => _isAttack;
        public override bool IsPlayer() => true;

        protected override void Start()
        {
            _interactables = new List<IInteractable>();

            _enemyOutlineRechanger = new EnemyOutlineRechanger();
            _getIsAttack = GetIsAttack;
            base.Start();
            SetCharacterData(characterData);
            InitializeTransition(new PlayerTransition(), _getIsAttack,null, splineFollower);
            InitializeInteractionSystem(cameraRay);
            SubscribeDeath();
            characterData.EventsInitialize(canvasController.UIDelegates.UpdateManaDelegate,
                canvasController.UIDelegates.UpdateHealthDelegate,
                canvasController.UIDelegates.UpdateEnergyDelegate);
        }

        private void FixedUpdate()
        {
            if(!_hasCharacter) return;
            projector.Project(transform.position, splineFollower.result);
        }

        protected override void SubscribeDeath()
        {
            base.SubscribeDeath();
            characterData.DieEvent += canvasController.Death;
        }

        public override void Finish()
        {
            canvasController.Death();
        }

        public  void UseBottle(EffectData data)
        {
            if(!_hasCharacter) return;
            BottleUseEvent?.Invoke(Receiver, data);
        }


        protected override void ClearPoint()
        {
            if(!_hasCharacter|| _currentPoint==null) return;
            characterData.DieEvent -= _currentPoint.GetDieCharacterDelegate();
            _interactables.Remove(_currentPoint);
            _currentPoint = null;
            _enemyOutlineRechanger.SetEnemy(null);
            StartCoroutine(RechangeCurrentPoint());
        }

        protected override void RemoveList(IInteractable enemy)
        {
            if(!_hasCharacter) return;

            if (_interactables.Contains(enemy)) _interactables.Remove(enemy);
        }

        protected override void StartRCP(List<IInteractable> points)
        {
            if(!_hasCharacter) return;
            AddToListEnemy(points);
        }

        private void AddToListEnemy(List<IInteractable> enemies)
        {
            if(!_hasCharacter) return;
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
            if(!_hasCharacter)  yield break;
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
                   SetCurrentPoint(point);
                }

                yield return new WaitForSeconds(0);
            }
        }

        protected override void SetCurrentPoint(IInteractable point)
        {
            if(!_hasCharacter) return;
            if (point.IsPlayer()) return;
            if (characterData.Energy > 1 && _currentPoint == point)
            {
                Attack();
            }

            if (_currentPoint != point)
            {
                _currentPoint = point;
                _enemyOutlineRechanger.SetEnemy(_currentPoint);
                base.SetCurrentPoint(point);
            }
        }

        public void OnButtonAttack()
        {
            Attack();
        }

        private async Task Attack()
        {
            _isAttack = true;
            await Task.Delay(100);
            WeaponAttack();
            await Task.Delay(100);
            _isAttack = false;
        }
        

        public override void SetOutline(bool value)
        {
        }

        public override void UseAbility(IAbilityCommand abilityCommand, int value)
        {
            if(!_hasCharacter) return;
            if (characterData.Mana <= 0) return;
            characterData.UseMana(value);
            base.UseAbility(abilityCommand, value);
        }
    }
}