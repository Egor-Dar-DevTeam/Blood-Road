using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Characters.AbilitiesSystem;
using Characters.Facades;
using UI;
using UI.CombatHUD;
using UnityEngine;

namespace Characters.Player
{
    public class PlayerController : BaseCharacter, IInteractable
    {
        public override bool IsPlayer() => true;
        
        [SerializeField] private Material outlineMaterial;
        [SerializeField] private CameraRay cameraRay;
        [SerializeField] private GameCanvasController canvasController;

        private List<IInteractable> _intractable;

        private event UpdateEnergyDelegate _updateEnergyEvent;
        private event UpdateHealthDelegate _updateHealthEvent;
        private event UpdateManaDelegate _updateManaEvent;

        private GetIsAttack _getIsAttack;

        private EnemyOutlineChanger _enemyOutlineChanger;
        
        private bool _isAttack;
        private bool GetIsAttack() => _isAttack;

        protected override void Start()
        {
            _intractable = new List<IInteractable>();

            _updateEnergyEvent += canvasController.UIDelegates.UpdateEnergyDelegate;

            _updateHealthEvent += canvasController.UIDelegates.UpdateHealthDelegate;
            _updateManaEvent += canvasController.UIDelegates.UpdateManaDelegate;
            
            _enemyOutlineChanger = new EnemyOutlineChanger(outlineMaterial);
            _getIsAttack = GetIsAttack;
            base.Start();
            InitializeTransition(new PlayerTransition(), _getIsAttack, _updateEnergyEvent);
            InitializeInteractionSystem(cameraRay);
            SubscribeDeath();
        }

        protected override void ClearPoint()
        {
            characterData.DieEvent -= _currentPoint.GetDieCharacterDelegate();
            _intractable.Remove(_currentPoint);
            _currentPoint = null;
            _enemyOutlineChanger.SetEnemy(null);
            
            ChangeCurrentPoint();
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
                if (_intractable.Contains(enemy)) continue;
                characterData.DieEvent += enemy.GetDieCharacterDelegate();
                _intractable.Add(enemy);
            }

            ChangeCurrentPoint();
        }

        private void ChangeCurrentPoint()
        {
            if (_currentPoint != null)
                return;
            if (_intractable.Count == 0)
                return;
             
            IInteractable point = null;
          
            for (int i = 0; i < _intractable.Count; i++)   
            {   
                if (_intractable[i].IsPlayer())
                    return; 
               
                int currentInteractableIndex = i;
                
                for (int j = 0; j < _intractable.Count; j++)
                {
                    if (IsFirstInteractableDistanceSmallerThanSecond(j, currentInteractableIndex))
                        currentInteractableIndex = j;
                }

                bool IsFirstInteractableDistanceSmallerThanSecond(int firstInteractable, int secondInteractable)
                {
                    return Vector3.Distance(transform.position, _intractable[firstInteractable].GetObject().position) <=
                           Vector3.Distance(transform.position, _intractable[secondInteractable].GetObject().position);
                }

                point = _intractable[currentInteractableIndex];
            }
 
             
            if (point != null && point.HasCharacter())
                SetEnemyPoint(point);
        }

        private void SetEnemyPoint(IInteractable point)
        {
            _currentPoint = point;

            _enemyOutlineChanger.SetEnemy(_currentPoint);
        }

        protected override async void SetCurrentPoint(IInteractable point)
        {
            if (point.IsPlayer())
                return; 
            
            if (_currentPoint != point)
            {
                _currentPoint = point;
                _enemyOutlineChanger.SetEnemy(_currentPoint);
            }

            if (characterData.Energy > 1)
            {
                await Attack();
            }
        }

        private async Task Attack()
        {
            _isAttack = true;
            await Task.Delay(200);
            _isAttack = false;
        }

        public override void ReceiveDamage(int value)
        {
            base.ReceiveDamage(value);
            _updateHealthEvent?.Invoke(characterData.Health);
        }

        public override void SetOutline(Material outline)
        {
        }

        public override void UseAbility(IAbilityCommand abilityCommand, int value)
        {
            if(characterData.Mana<=0)return;
            characterData.UseMana(value);
            _updateManaEvent?.Invoke(characterData.Mana);
            base.UseAbility(abilityCommand, value);
        }
    }
}