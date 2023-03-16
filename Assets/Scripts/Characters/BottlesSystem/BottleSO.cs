using Banks;
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
        [SerializeField] private int currentCount;
        private BottleInfo _bottleInfo;
        private BankDelegates _bankDelegates;
        public BottleInfo BottleInfo => new BottleInfo(_bottleInfo.Sprite, _bottleInfo.Cooldown, currentCount);
        public EffectData EffectData => EffectData.From(data);
        public BankDelegates BankDelegates => _bankDelegates;

        public void Initialize()
        {
            var bottleBank = new Bottle();
            bottleBank.Initialize(name);
            _bankDelegates = bottleBank.Delegates;
           // _bankDelegates.Add.Invoke(currentCount);
            _bottleInfo = new BottleInfo(sprite, cooldown, currentCount);
        }
    }

    public struct BottleInfo
    {
        public Sprite Sprite { get; }
        public int Cooldown { get; }
        public int CurrentCount { get; }
        public BottleInfo(Sprite sprite, int cooldown, int currentCount)
        {
            Sprite = sprite;
            Cooldown = cooldown;
            CurrentCount = currentCount;
        }
    }
}