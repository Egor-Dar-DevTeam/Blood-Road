using System.Collections.Generic;
using System.Linq;
using Characters.AbilitiesSystem;
using Characters.Facades;
using Characters.Player;
using Interaction;
using UnityEngine;

namespace Characters.Enemy
{
    public class DefaultEnemy : BaseCharacter
    {
        [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
        [SerializeField] private Money moneyPrefab;
        public override bool IsPlayer() => false;
        protected event RemoveList _removeList;

        public override void SetCharacterData(CharacterData data)
        {
            base.SetCharacterData(data);
            base.Awake();
            InitializeTransition(new EnemyTransition(), null, null, moneyPrefab, characterData.GetRecoilDelegate);
            InitializeAbility(new AbilityData(VFXTransforms, characterData.ImpenetrableDelegate,
                mapStates, iDCharacter));
            InitializeInteractionSystem(null);
            SubscribeDeath();
            SubscribeCharacterData();
            CharacterDataSubscriber.DieEvent += Die;
        }

        protected override void InitializeAbility(AbilityData abilityData)
        {
            var data = _transitionAndStates.ReturnReadyData(abilityData);
            _transitionAndStates.InitializeAbilities(new AbilitiesSystem.Enemy(data));
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
            //   CharacterDataSubscriber.DieEvent -= Die;
            _removeList?.Invoke(this);
        }
    }
}