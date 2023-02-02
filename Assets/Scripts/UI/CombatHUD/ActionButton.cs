using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.CombatHUD
{
    public class ActionButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private Image image;
        [SerializeField] private Image cooldownMask;

        public void Initialize(float cooldown, UnityAction action, Sprite sprite)
        {
            image.sprite = sprite;
            image.color = Color.white;
            button.onClick.AddListener( async ()=>InteractableButton(cooldown, action));
        }

        private async void InteractableButton(float cooldown, UnityAction action)
        {
            action.Invoke();
            button.interactable = false;
            var scale = button.transform.localScale;
            var runTween = DOTween.Sequence()
                .Append(cooldownMask.DOFillAmount(1, 0.2f))
                .Append(cooldownMask.DOFillAmount(0, cooldown))
                .Append(button.transform.DOScale( new Vector3(scale.x + 0.1f, scale.y+0.1f,scale.z), 0.2f))
                .Append(button.transform.DOScale(new Vector3(scale.x, scale.y,scale.z), 0.2f));
            runTween.Play();
                  await runTween.AsyncWaitForCompletion();
            button.interactable = true;
        }
    }
}