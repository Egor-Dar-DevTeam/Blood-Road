using UI.CombatHUD;
using UnityEngine;

namespace UI
{
    public class GameCanvasController : MonoBehaviour
    {
        [SerializeField] private CombatHUD.CombatHUD combatHUD;
        public UIDelegates UIDelegates => _uiDelegates;
        private UIDelegates _uiDelegates;

        
        private UpdateEnergyDelegate _updateEnergyDelegate;
        private UpdateManaDelegate _updateManaDelegate;
        private UpdateHealthDelegate _updateHealthDelegate;

        private void Awake()
        {
            _updateManaDelegate = combatHUD.SetMana;
            _updateHealthDelegate = combatHUD.SetHealth;
            _updateEnergyDelegate = combatHUD.SetEnergy;
            _uiDelegates = new UIDelegates(_updateEnergyDelegate, _updateManaDelegate, _updateHealthDelegate);
        }
        
    }

    public struct UIDelegates
    {
        public readonly UpdateEnergyDelegate UpdateEnergyDelegate;
        public readonly UpdateManaDelegate UpdateManaDelegate;
        public readonly UpdateHealthDelegate UpdateHealthDelegate;

        public UIDelegates(UpdateEnergyDelegate updateEnergyDelegate, UpdateManaDelegate updateManaDelegate,
            UpdateHealthDelegate updateHealthDelegate)
        {
            UpdateManaDelegate = updateManaDelegate;
            UpdateHealthDelegate = updateHealthDelegate;
            UpdateEnergyDelegate = updateEnergyDelegate;
        }
    }
}