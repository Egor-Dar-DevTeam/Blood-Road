using Characters.Player;
using UI.CombatHUD;

namespace UI
{
    public struct UIDelegates
    {
        public readonly UpdateEnergyDelegate UpdateEnergyDelegate;
        public readonly UpdateManaDelegate UpdateManaDelegate;
        public readonly UpdateHealthDelegate UpdateHealthDelegate;
        public readonly DieDelegate DieDelegate;

        public UIDelegates(UpdateEnergyDelegate updateEnergyDelegate, UpdateManaDelegate updateManaDelegate,
            UpdateHealthDelegate updateHealthDelegate, DieDelegate dieDelegate)
        {
            UpdateManaDelegate = updateManaDelegate;
            UpdateHealthDelegate = updateHealthDelegate;
            UpdateEnergyDelegate = updateEnergyDelegate;
            DieDelegate = dieDelegate;
        }
    }
}