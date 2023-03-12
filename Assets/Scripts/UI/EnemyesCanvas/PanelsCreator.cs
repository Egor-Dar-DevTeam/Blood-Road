using UnityEngine;

namespace UI.EnemyesCanvas
{
    public class PanelsCreator : MonoBehaviour
    {
        [SerializeField] private PanelHealth prefab;
        [SerializeField] private Canvas current;

        private void Update()
        {
            if(current.worldCamera==null)
            current.worldCamera = Camera.main;
        }

        public UIDelegates AddCharacter(Transform character)
        {
            var panel = Instantiate(prefab, transform);
            var delegatesCharactersInfo = new UIDelegatesCharactersInfo();
            delegatesCharactersInfo.SetDelegates(panel.InfoCharacterResourceBar.SetEnergy,
                panel.InfoCharacterResourceBar.SetMana, panel.InfoCharacterResourceBar.SetHealth, panel.Destroy);
            panel.FollowPointPanel.SetPoint(character);
            return delegatesCharactersInfo.Delegates();
        }
    }
}