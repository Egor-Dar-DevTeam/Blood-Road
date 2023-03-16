using Banks;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
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

        public void Initialize(float cooldown, UnityAction action, Sprite sprite, BankDelegates? delegates)
        {
            image.sprite = sprite;
            image.color = Color.white;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => InteractableButton(cooldown, action));
            if(delegates==null) return;
            BankDelegates bank = (BankDelegates)delegates;
            _currentCount = bank.Value;
            if (currentCountText == null && _currentCount == 0) return;
            currentCountText.text = _currentCount.ToString();
            button.onClick.AddListener((() =>
            {
                bank.Remove.Invoke(1);
                _currentCount = bank.Value;
                currentCountText.text = _currentCount.ToString();
            }));
        }

        private async void InteractableButton(float cooldown, UnityAction action)
        {
            if (button == null) return;
//            if (currentCountText!=null && _currentCount <= 0) return;
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