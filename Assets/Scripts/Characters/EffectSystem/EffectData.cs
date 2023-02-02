using System;

namespace Characters.EffectSystem
{
    [Serializable]
    public struct EffectData
    {
        public int HealthDamage { get; }
        public int MagicDamage { get; }
        public float Duration { get; }
        public int HealthAdd { get; }
        public int ManaAdd { get; }
        public int EnergyAdd { get; }
        public Type Type { get; }

        public EffectData(int healthDamage, int magicDamage, float duration,
            int healthAdd, int manaAdd, int energyAdd, Type type)
        {
            HealthDamage = healthDamage;
            MagicDamage = magicDamage;
            Duration = duration;
            Type = type;
            HealthAdd = healthAdd;
            ManaAdd = manaAdd;
            EnergyAdd = energyAdd;
        }

        public static EffectData From(PassiveData data, Type type = null)
        {
            return new EffectData(0, 0, 0, data.HealthAdd, data.ManaAdd, data.EnergyAdd,
                type);
        }

    }
}