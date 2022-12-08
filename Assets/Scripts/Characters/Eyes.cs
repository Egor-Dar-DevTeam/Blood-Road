using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Enemy;
using Characters.Player;
using UnityEngine;

namespace Characters
{
    public delegate void SetPoint(List<IInteractable> point);

    public class Eyes : MonoBehaviour, IInit<SetPoint>
    {
        private event SetPoint _setPoint;
        private List<IInteractable> _interactables= new List<IInteractable>();
        public void Initialize(SetPoint setPointDelegate)
        {
            _setPoint = setPointDelegate;
            setPointDelegate += _setPoint;
            StartCoroutine(Notification());
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.TryGetComponent(out IInteractable interactable))
            {
                if (!_interactables.Contains(interactable))
                {
                    _interactables.Add(interactable);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.TryGetComponent(out IInteractable interactable))
            {
                if (_interactables.Contains(interactable))
                {
                    _interactables.Remove(interactable);
                }
            }
        }

        private IEnumerator Notification()
        {
            for (int i = 0; i < 1;)
            {
                yield return new WaitForSeconds(0.1f);
                _setPoint?.Invoke(_interactables);
            }
        }
    }
}