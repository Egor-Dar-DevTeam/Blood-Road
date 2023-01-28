using System;

namespace Characters.EffectSystem
{
    [Serializable]
    public struct EffectData
    {
        public int HealthDamage { get; }
        public int MagicDamage { get; }
        public float Duration { get; }
        public int PassiveSkill { get; }
        public int NegativeSkill { get; }

        public EffectData(int healthDamage, int magicDamage, float duration,
            int passiveSkill, int negativeSkill)
        {
            HealthDamage = healthDamage;
            MagicDamage = magicDamage;
            Duration = duration;
            PassiveSkill = passiveSkill;
            NegativeSkill = negativeSkill;
        }

    }
}