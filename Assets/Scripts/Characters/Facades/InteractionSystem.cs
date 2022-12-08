using System.Collections.Generic;
using Characters.Player;
using UnityEngine;

namespace Characters.Facades
{
    public class InteractionSystem
    {
        private IInit<SetCurrentPoint> _instCameraRay;
        private IInit<SetPoint> _instEyes;
        private List<IInteractable> _points;
        private SetPoint _setPoint;
        private SetCurrentPoint _setCurrentPoint;
        private event SetCurrentPoint _setCurrentPointInPlayerEvent;
        private event StartRechangeCurrentPoint _rechangeCurrentPointInPlayerEvent;
        
        public void Initialize(CameraRay cameraRay, Eyes eyesCharacters, Transform transform, SetCurrentPoint setCurrentPointDelegate, StartRechangeCurrentPoint rechangeCurrentPointDelegate)
        {
            _setCurrentPointInPlayerEvent = setCurrentPointDelegate;
            setCurrentPointDelegate += _setCurrentPointInPlayerEvent;

            _rechangeCurrentPointInPlayerEvent = rechangeCurrentPointDelegate;
            rechangeCurrentPointDelegate += _rechangeCurrentPointInPlayerEvent;
            
            _points = new List<IInteractable>();

            _setPoint = SetPoint;
            _setCurrentPoint = SetCurrentPoint;

            _instCameraRay = Object.Instantiate(cameraRay);
            _instCameraRay.Initialize(_setCurrentPoint);
            
            _instEyes = Object.Instantiate(eyesCharacters, transform);
            _instEyes.Initialize(_setPoint);
        }
        private void SetPoint(List<IInteractable> points)
        {
            for (int i = 0; i < points.Count; i++)
            {
                if (!_points.Contains(points[i]))
                {
                    _points.Add(points[i]);
                }
            }
            _rechangeCurrentPointInPlayerEvent?.Invoke(_points);
            //TODO: Сделать вызов системы выбора ближайшего врага
        }

        private void SetCurrentPoint(IInteractable point)
        {
            if (!_points.Contains(point))
            {
                _points.Add(point);
            }
            _setCurrentPointInPlayerEvent?.Invoke(point);

        }
    }
}