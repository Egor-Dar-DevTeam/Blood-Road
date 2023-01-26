using Characters.AbilitiesSystem;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

namespace Characters.EffectSystem
{
    public class Sender
    {
        public void Register(EffectData effectData)
        {
            
        }
    }

    public struct EffectData
    {
        public int EffectDamage { get; }
        public int MagicDamage { get; }
        public float Duration { get; }
        public int PassiveSkill { get; }
        public int NegativeSkill { get; }
    }
}