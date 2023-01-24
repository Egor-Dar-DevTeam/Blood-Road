using System;
using Better.Attributes.Runtime.Select;
using Characters.AbilitiesSystem.Declaration;
using UnityEngine;

namespace Characters.AbilitiesSystem.Ability
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Ability", fileName = "Ability", order = 0)]
    public class AbilitySO : ScriptableObject
    {
        [SelectImplementation(typeof(IAbilityCommand))] [SerializeReference]
        private IAbilityCommand command;

        [SerializeField] private Sprite sprite;
        [SerializeField] private int cooldown;
        [SerializeField] private int price;
        private AbilityInfo info;
        public AbilityInfo AbilityInfo => info;

        public void Initialize()
        {
            info = new AbilityInfo(sprite, cooldown, command, price);
        }
    }

    [Serializable]
    public struct AbilityInfo
    {
        public Sprite Sprite { get; }
        public int Cooldown { get; }
        public IAbilityCommand AbilityCommand { get; }
        public int Price { get; }

        public AbilityInfo(Sprite sprite, int cooldown, IAbilityCommand abilityCommand, int price)
        {
            Sprite = sprite;
            Cooldown = cooldown;
            AbilityCommand = abilityCommand;
            Price = price;
        }
    }
}