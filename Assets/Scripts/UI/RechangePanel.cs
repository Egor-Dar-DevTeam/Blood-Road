using UnityEngine;

namespace UI
{
    public class RechangePanel
    {
        private CanvasGroup _currentCanvasGroup;

        public void SetNewPanel(CanvasGroup newPanel)
        {
            if (_currentCanvasGroup != null)
            {
                _currentCanvasGroup.alpha = 0;
                _currentCanvasGroup.interactable = false;
                _currentCanvasGroup.blocksRaycasts = false;
            }

            _currentCanvasGroup = newPanel;
            _currentCanvasGroup.alpha = 1;
            _currentCanvasGroup.interactable = true;
            _currentCanvasGroup.blocksRaycasts = true;
        }
    }
}