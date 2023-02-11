using UnityEngine;

namespace Cinemachine
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private TriggerAddPriority[] triggerAddPriorities;

        private void Start()
        {
            for (int i = 0; i < triggerAddPriorities.Length; i++)
            {
                triggerAddPriorities[i].Initialize(this);
            }
        }

        private CinemachineVirtualCamera _currentCinemachineVirtualCamera;

        public void SetCamera(CinemachineVirtualCamera newCinemachineVirtualCamera)
        {
            if (_currentCinemachineVirtualCamera != null)
            {
                _currentCinemachineVirtualCamera.Priority = 0;
            }

            _currentCinemachineVirtualCamera = newCinemachineVirtualCamera;
            _currentCinemachineVirtualCamera.Priority = 1;
        }
    }
}