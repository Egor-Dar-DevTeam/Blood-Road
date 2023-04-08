using Banks;
using DG.Tweening;
using MapSystem.Structs;
using TMPro;
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

        [Header("optional field")] [SerializeField]
        private TextMeshProUGUI currentCountText;

        private int _currentCount;

        public void Initialize(UnityAction action, UIInfo info)
        {
            image.sprite = info.Sprite;
            image.color = Color.white;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => InteractableButton(info.Cooldown, action));
        }

        public void Initialize( UnityAction action,UIInfo info, Remove remove)
        {
            Initialize(action, info);
            currentCountText.text = _currentCount.ToString();
            button.onClick.AddListener((() =>
            {
                remove?.Invoke(1);
            }));
        }

        public void SetValue(int value)
        {
            _currentCount = value;
            currentCountText.text = _currentCount.ToString();
            button.interactable = _currentCount > 0;
        }

        private async void InteractableButton(float cooldown, UnityAction action)
        {
            if (button == null) return;
            action?.Invoke();
            if (cooldown != 0)
            {
                button.interactable = false;
                var scale = button.transform.localScale;
                var runTween = DOTween.Sequence()
                    .Append(cooldownMask.DOFillAmount(1, 0.2f))
                    .Append(cooldownMask.DOFillAmount(0, cooldown))
                    .Append(button.transform.DOScale(new Vector3(scale.x + 0.1f, scale.y + 0.1f, scale.z), 0.2f))
                    .Append(button.transform.DOScale(new Vector3(scale.x, scale.y, scale.z), 0.2f));
                runTween.Play();
                await runTween.AsyncWaitForCompletion();
            }

            button.interactable = true;
        }
    }
}