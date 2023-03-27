using System;
using System.Collections.Generic;
using Characters.Information.Structs;
using Unity.VisualScripting;
using UnityEngine;

namespace Characters.Information
{
    public class StatesInfo : MonoBehaviour
    {
        [SerializeField] private CurrentStateInfo[] currentStatesInfo;
        private Dictionary<Type, StateInfo> _states;

        public StateInfo GetState(Type nameState)
        {
            if (_states != null) return _states[nameState];
            _states = new Dictionary<Type, StateInfo>();
            foreach (var state in currentStatesInfo)
            {
                _states.Add(state.StateName, state.StateInfo);
            }
            return _states[nameState];
        }

        private void Awake()
        {
            _states = new Dictionary<Type, StateInfo>();
            foreach (var state in currentStatesInfo)
            {
                _states.Add(state.StateName, state.StateInfo);
            }
        }
    }
}