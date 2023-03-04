using System;
using Better.Attributes.Runtime.Select;
using Unity.VisualScripting;
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
        [SerializeField] private string name;
        [SerializeField] private string description;
        private bool _isUsed;
        private bool _initialized;
        private AbilityInfo _info;
        private AbilityUIInfo _UIInfo;

        public AbilityInfo AbilityInfo => _info;
        public AbilityUIInfo AbilityUIInfo => _UIInfo;
        public bool IsUsed => _isUsed;
        
        public void Used(bool value)
        {
            _isUsed = value;
        }

        public void Initialize()
        {
            _info = new AbilityInfo(sprite, cooldown, command, price);
            _UIInfo = new AbilityUIInfo(name, description, sprite);
        }

        private void OnDestroy()
        {
            _initialized = false;
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

    public struct AbilityUIInfo
    {
        public string Name { get; }
        public string Description { get; }
        public Sprite Sprite { get; }

        public AbilityUIInfo(string name, string description, Sprite sprite)
        {
            Name = name;
            Description = description;
            Sprite = sprite;
        }
    }
}