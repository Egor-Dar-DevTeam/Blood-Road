using UnityEngine;

namespace Characters.Player
{
    public delegate void SetCurrentPoint(IInteractable point);
    public class CameraRay : MonoBehaviour, IInit<SetCurrentPoint>
    {
        [SerializeField] private new Camera camera;
        private event SetCurrentPoint _setPoint;

        void Update()
        {
            if (!Input.GetKeyDown(KeyCode.Mouse0)) return;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out var hit)) return;
            if (hit.collider.gameObject.TryGetComponent(out IInteractable enemy))
            {
                _setPoint?.Invoke(enemy);
            }
        }

        public void Initialize(SetCurrentPoint setCurrentPoint)
        {
            _setPoint = setCurrentPoint;
            setCurrentPoint += _setPoint;
        }
    }
}