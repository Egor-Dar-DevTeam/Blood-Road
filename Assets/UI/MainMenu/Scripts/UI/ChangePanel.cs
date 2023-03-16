using DG.Tweening;
using UnityEngine;


namespace UI
{
    public class ChangePanel
    {
        private CanvasGroup _panel;


        public void SetPanel(CanvasGroup newPanel)
        {
            if (_panel != null)
            {
                _panel.DOFade(0f, 0.5f);
                _panel.interactable = false;
                _panel.blocksRaycasts = false;
            }

            _panel = newPanel;
            _panel.DOFade(1f, 0.5f);
            _panel.interactable = true;
            _panel.blocksRaycasts = true;
        }
    }
}