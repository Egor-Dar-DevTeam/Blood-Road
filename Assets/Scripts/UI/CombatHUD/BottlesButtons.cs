using Characters.BottlesSystem;
using Characters.Player;
using UnityEngine;

namespace UI.CombatHUD
{
    public class BottlesButtons : MonoBehaviour
    {
        [SerializeField] private ActionButton[] buttons;
        [SerializeField] private BottleSO[] bottleSo;
        [SerializeField] private PlayerController playerController;

        private void Awake()
        {
            for (var i = 0; i < buttons.Length; i++)
            {
                bottleSo[i].Initialize();
                var info = bottleSo[i].BottleInfo;
                var effectData = bottleSo[i].EffectData;
                var delegates = bottleSo[i].BankDelegates;
                buttons[i].Initialize(info.Cooldown, (() => playerController.UseBottle(effectData)), info.Sprite,
                    delegates.Remove);
                delegates.InitGetValue.Subscribe(buttons[i].SetValue);
            }
        }

        private void OnDestroy()
        {
            for (var i = 0; i < buttons.Length; i++)
            {
                var delegates = bottleSo[i].BankDelegates;
                delegates.InitGetValue.Unsubscribe(buttons[i].SetValue);
            }
        }
    }
}