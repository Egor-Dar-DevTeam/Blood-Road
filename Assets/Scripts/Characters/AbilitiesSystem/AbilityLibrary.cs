using System;
using System.Collections.Generic;
using Characters.AbilitiesSystem.States;
using Characters.MapperSystem;
using UnityEngine.Rendering.Universal;

namespace Characters.AbilitiesSystem
{
    public  class AbilityLibrary : MapperBase<Type, Type>
    {
        private static AbilityLibrary _abilityLibraryInstance;
        private  Dictionary<Type, BaseState> _baseStates;
        private  Dictionary<Type, IAbilityCommand> _abilityCommands;
        private  Dictionary<Type, int> _idStates;
        public  int ToEffectID(Type abilityCommand) => _idStates[abilityCommand];

        public AbilityLibrary() : base()
        {
            _baseStates = new Dictionary<Type, BaseState>();
            _abilityCommands = new Dictionary<Type, IAbilityCommand>();
            _idStates = new Dictionary<Type, int>();
        }

        private void AddAbility(Type key, IAbilityCommand value)
        {
            if (!_abilityCommands.ContainsKey(key))
            {
                _abilityCommands.Add(key,value);
                Map(_abilityCommands, _baseStates);
            }
        }

        private void AddState(Type key, BaseState value)
        {
            if (!_baseStates.ContainsKey(key))
            {
                _baseStates.Add(key,value);
                AddID(key, value.ID);
                Map(_abilityCommands, _baseStates);
            }
        }

        public static void StaticAddAbility(Type key, IAbilityCommand value)
        {
            _abilityLibraryInstance.AddAbility(key,value);
        }

        public static void StaticAddState(Type key, BaseState value)
        {
            _abilityLibraryInstance.AddState(key, value);
        }

        private void AddID(Type key, int value)
        {
            if(!_idStates.ContainsKey(key)) _idStates.Add(key,value);
        }

        private void Map(Dictionary<Type, IAbilityCommand> abilityCommands, Dictionary<Type, BaseState> baseStates)
        {
            foreach (var t in abilityCommands)
            {
                var valueType = t.Value.GetType();

                if (baseStates.TryGetValue(valueType, out var instance) && t.Key == instance.GetType())
                {
                    if (!_dictionary.ContainsKey(valueType))
                    {
                        _dictionary.Add(valueType, instance.GetType());
                        _idStates.Add(valueType, instance.ID);
                        _abilityCommands.Add(instance.GetType(), t.Value);
                        _baseStates.Add(valueType, instance);
                    }
                }
            }
        }
    }
}