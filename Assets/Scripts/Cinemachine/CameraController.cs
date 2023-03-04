using UnityEngine;

namespace Cinemachine
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private TriggerAddPriority[] triggerAddPriorities;
        [SerializeField] private CinemachineVirtualCamera abilityCamera;
        private CinemachineVirtualCamera _currentCinemachineVirtualCamera;

        private void Start()
        {
            foreach (var trigger in triggerAddPriorities)
            {
                trigger.Initialize(this);
            }
        }

        public void SetCamera(CinemachineVirtualCamera newCinemachineVirtualCamera)
        {
            if (_currentCinemachineVirtualCamera != null)
            {
                _currentCinemachineVirtualCamera.Priority = 0;
            }

            _currentCinemachineVirtualCamera = newCinemachineVirtualCamera;
            _currentCinemachineVirtualCamera.Priority = 1;
        }

        public void AbilityCamera(bool value)
        {
            if (value)
            {
                _currentCinemachineVirtualCamera.Priority = 0;
                abilityCamera.Priority = 1;
            }
            else
            {
                _currentCinemachineVirtualCamera.Priority = 1;
                abilityCamera.Priority = 0;
            }
        }
    }
}