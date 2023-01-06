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
        private UpdateShieldDelegate _updateShieldDelegate;
        private UpdateHealthDelegate _updateHealthDelegate;

        private void Awake()
        {
            _updateShieldDelegate = combatHUD.SetShield;
            _updateHealthDelegate = combatHUD.SetHealth;
            _updateEnergyDelegate = combatHUD.SetEnergy;
            _uiDelegates = new UIDelegates(_updateEnergyDelegate, _updateShieldDelegate, _updateHealthDelegate);
        }
        
    }

    public struct UIDelegates
    {
        public readonly UpdateEnergyDelegate UpdateEnergyDelegate;
        public readonly UpdateShieldDelegate UpdateShieldDelegate;
        public readonly UpdateHealthDelegate UpdateHealthDelegate;

        public UIDelegates(UpdateEnergyDelegate updateEnergyDelegate, UpdateShieldDelegate updateShieldDelegate,
            UpdateHealthDelegate updateHealthDelegate)
        {
            UpdateShieldDelegate = updateShieldDelegate;
            UpdateHealthDelegate = updateHealthDelegate;
            UpdateEnergyDelegate = updateEnergyDelegate;
        }
    }
}