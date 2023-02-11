using Characters.Player;
using UI.CombatHUD;

namespace UI
{
    public class UIDelegatesCharactersInfo
    {
        private UpdateEnergyDelegate _updateEnergyDelegate;
        private UpdateManaDelegate _updateManaDelegate;
        private UpdateHealthDelegate _updateHealthDelegate;
        private DieDelegate _dieDelegate;


        public void SetDelegates(UpdateEnergyDelegate updateEnergyDelegate,
            UpdateManaDelegate updateManaDelegate, UpdateHealthDelegate updateHealthDelegate, DieDelegate dieDelegate = null)
        {
            _updateManaDelegate = updateManaDelegate;
            _updateHealthDelegate = updateHealthDelegate;
            _updateEnergyDelegate = updateEnergyDelegate;
            _dieDelegate = dieDelegate;
        }

        public UIDelegates Delegates()
        {
            return new UIDelegates(_updateEnergyDelegate, _updateManaDelegate, _updateHealthDelegate, _dieDelegate);
        }
    }
}