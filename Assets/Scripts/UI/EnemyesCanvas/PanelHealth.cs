using UI.CombatHUD;
using UnityEngine;

namespace UI.EnemyesCanvas
{
    public class PanelHealth: MonoBehaviour
    {
        [SerializeField] private InfoCharactersResourceBars infoCharacterResourceBar;
        [SerializeField] private FollowPointPanel followPointPanel;
        public InfoCharactersResourceBars InfoCharacterResourceBar => infoCharacterResourceBar;
        public FollowPointPanel FollowPointPanel => followPointPanel;

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}