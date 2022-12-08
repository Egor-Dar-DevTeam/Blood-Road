using UnityEngine;

namespace Characters.Enemy
{
    public class DefaultEnemy : MonoBehaviour, IInteractable
    {
        [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
        

        public void SetOutline(Material outline)
        {
            skinnedMeshRenderer.material = outline;
        }

        public Transform GetObject() => this.transform;
    }
}
