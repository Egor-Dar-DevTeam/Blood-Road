using Characters.EffectSystem;
using UnityEngine;

namespace Characters.WeaponSystem
{
    public abstract class Weapon : MonoBehaviour
    {
        [SerializeField] private EffectData effectData;
        [SerializeField] private int id;
        public EffectData EffectData => effectData;
        public int ID => id;
    }
}