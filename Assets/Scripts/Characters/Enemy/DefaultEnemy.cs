using System.Collections.Generic;
using System.Linq;
using Characters.Facades;
using Characters.InteractableSystems;
using Characters.Player;
using UnityEngine;

namespace Characters.Enemy
{
    public class DefaultEnemy : BaseCharacter, IInteractable, IInit<DieInteractable>
    {
        [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
        [SerializeField] private Money moneyPrefab;
        public override bool IsPlayer() => false;
        protected event RemoveList _removeList;

        protected override void Start()
        {
            base.Start();
            InitializeTransition(new EnemyTransition(), null, null, moneyPrefab);
            InitializeInteractionSystem(null);
            SubscribeDeath();
            SubscribeDeathMethod(Die);
        }

        protected override void ClearPoint(IInteractable interactable)
        {
            if (_currentPoint == null) return;
            characterData.DieInteractable -= _currentPoint.GetDieCharacterDelegate;
            _currentPoint = null;
        }

        protected override void StartRCP(List<IInteractable> points)
        {
            foreach (var point in points.Where(point => point.IsPlayer()))
            {
                SetCurrentPoint(point);
            }
        }

        protected override void SetCurrentPoint(IInteractable point)
        {
            if (_currentPoint != null || _currentPoint == point || !point.HasCharacter()) return;
            _currentPoint = point;
            characterData.DieInteractable += _currentPoint.GetDieCharacterDelegate;
            _removeList += _currentPoint.GetRemoveList();
            base.SetCurrentPoint(point);
        }

        public override void SetOutline(bool value)
        {
            for (int i = 0; i < skinnedMeshRenderer.materials.Length; i++)
            {
                skinnedMeshRenderer.materials[i].SetFloat("Vector1_2A6393C8", value ? 0.5f : 2f);
            }
        }

        private void Die()
        {
            _removeList?.Invoke(this);
        }

        public void Initialize(DieInteractable subscriber)
        {
            characterData.DieInteractable += subscriber;
        }
    }
}