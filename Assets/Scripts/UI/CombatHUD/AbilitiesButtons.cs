using Characters.AbilitiesSystem;
using Characters.AbilitiesSystem.Ability;
using Characters.AbilitiesSystem.Declaration;
using Characters.Player;
using UnityEngine;

namespace UI.CombatHUD
{
    public class AbilitiesButtons : MonoBehaviour
    {
        [SerializeField] private ActionButton[] button;
        [SerializeField] private AbilitySO[] abilitySo;
        [SerializeField] private PlayerController playerController;

        private void Awake()
        {
            for (int i = 0; i < button.Length; i++)
            {
                abilitySo[i].Initialize();
                var info = abilitySo[i].AbilityInfo;
                button[i].Initialize(info.Cooldown, ()=>playerController.UseAbility(info.AbilityCommand, info.Price), info.Sprite);
            }
        }
    }
}