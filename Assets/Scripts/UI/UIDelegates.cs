using UI.CombatHUD;

namespace UI
{
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