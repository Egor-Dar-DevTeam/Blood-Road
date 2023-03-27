using System.Collections.Generic;
using Characters.AbilitiesSystem.Ability;
using Characters.Player;
using UnityEngine;

namespace UI.CombatHUD
{
    public class AbilitiesButtons : MonoBehaviour
    {
        [SerializeField] private List<ActionButton> button;
        [SerializeField] private List<AbilitySO> abilitiesSo;
        [SerializeField] private PlayerController playerController;
        public void AddAbility(AbilitySO abilitySo)
        {
            if (abilitiesSo.Contains(abilitySo)) return;
                abilitiesSo.Add(abilitySo);
            RecheckAbilities();
        }

        public List<AbilitySO> GetCopy() => new(abilitiesSo);
        

        public void RemoveAbility(AbilitySO abilitySo)
        {
            abilitiesSo.Remove(abilitySo);
        }
        
        private void Awake()
        {
            RecheckAbilities();
        }

        private void RecheckAbilities()
        {
            for (var i = 0; i < button.Count; i++)
            {
                if (abilitiesSo.Count == 0|| abilitiesSo.Count-1<i) continue;
                abilitiesSo[i].Initialize();
                abilitiesSo[i].Used(true);
                var info = abilitiesSo[i].AbilityInfo;
                button[i].Initialize(info.Cooldown,
                    () => playerController.UseAbility(info.AbilityCommand, info.Price),
                    info.Sprite);
            }
        }
    }
}