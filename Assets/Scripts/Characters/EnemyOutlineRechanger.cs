﻿using UnityEngine;

namespace Characters
{
    public class EnemyOutlineRechanger
    {
        private IInteractable _currentEnemyOutline;
        private Material _outlineMaterial;
        public EnemyOutlineRechanger(Material outlineMaterial )
        {
            _outlineMaterial = outlineMaterial;
        }

        public void SetEnemy(IInteractable newEnemy)
        {
            _currentEnemyOutline?.SetOutline(false);
            _currentEnemyOutline = newEnemy;
            _currentEnemyOutline?.SetOutline(true);
        }
    }
}