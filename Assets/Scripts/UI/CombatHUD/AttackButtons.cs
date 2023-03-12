using Characters.Player;
using UnityEngine;

namespace UI.CombatHUD
{
    public class AttackButtons : MonoBehaviour
    {
        [SerializeField] private ActionButton[] buttons;
        [SerializeField] private Sprite attackButtonSprite;
        [SerializeField] private AttackVariants attackVariants;

        private void Start()
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                var a = i;
                buttons[i].Initialize(0,()=> attackVariants.Attack(a),attackButtonSprite);
            }
        }
        
    }
}
