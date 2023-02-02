using Characters.EffectSystem;
using UnityEngine;

namespace Characters.BottlesSystem
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Bottle", fileName = "Bottle", order = 3)]
    public class BottleSO : ScriptableObject
    {
        [SerializeField] private Sprite sprite;
        [SerializeField] private int cooldown;
        [SerializeField] private PassiveData data;
        private BottleInfo _bottleInfo;
        public BottleInfo BottleInfo => new BottleInfo(_bottleInfo.Sprite, _bottleInfo.Cooldown);
        public EffectData EffectData => EffectData.From(data);

        public void Initialize()
        {
            _bottleInfo = new BottleInfo(sprite, cooldown);
        }
    }

    public struct BottleInfo
    {
        public Sprite Sprite { get; }
        public int Cooldown { get; }

        public BottleInfo(Sprite sprite, int cooldown)
        {
            Sprite = sprite;
            Cooldown = cooldown;
        }
    }
}