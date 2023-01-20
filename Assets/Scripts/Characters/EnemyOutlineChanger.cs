using UnityEngine;

namespace Characters
{
    public class EnemyOutlineChanger
    {
        private IInteractable _currentEnemyOutline;
        
        private readonly Material _outlineMaterial;
        
        public EnemyOutlineChanger(Material outlineMaterial)
        {
            _outlineMaterial = outlineMaterial;
        }

        public void SetEnemy(IInteractable newEnemy)
        {
            _currentEnemyOutline?.SetOutline(null);
            _currentEnemyOutline = newEnemy;
            _currentEnemyOutline?.SetOutline(_outlineMaterial);
        }
    }
}