using Cinemachine;
using UnityEngine;

namespace Characters.AbilitiesSystem
{
    public class AbilityTrigger : MonoBehaviour
    {
       [SerializeField] private CameraController _cameraController;

        public void SetCameraController(CameraController cameraController)
        {
            _cameraController = cameraController;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out ITriggerable triggerable))
            {
                _cameraController.AbilityCamera(true);
                triggerable.AbilityTrigger();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out ITriggerable triggerable))
            {
                _cameraController.AbilityCamera(false);
            }
        }
    }
}