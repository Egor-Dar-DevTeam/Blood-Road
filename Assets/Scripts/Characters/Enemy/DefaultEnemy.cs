using System;
using System.Collections.Generic;
using System.Linq;
using Characters.Facades;
using Characters.Player;
using UnityEngine;

namespace Characters.Enemy
{
    public class DefaultEnemy : BaseCharacter
    {
        [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
        public override bool IsPlayer() => false;
        private event RemoveList _removeList;

        private void Awake()
        {
            capsuleCollider.radius *= 2;
            capsuleCollider.height *= 2;
        }

        protected override void Start()
        {
            base.Start();
            InitializeTransition(new EnemyTransition(), null);
            InitializeInteractionSystem(null);
            SubscribeDeath();
            SubscribeDeathMethod(Die);
        }

        protected override void ClearPoint()
        {
            if(_currentPoint==null) return;
            characterData.DieEvent -= _currentPoint.GetDieCharacterDelegate();
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
            characterData.DieEvent += _currentPoint.GetDieCharacterDelegate();
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
    }
}