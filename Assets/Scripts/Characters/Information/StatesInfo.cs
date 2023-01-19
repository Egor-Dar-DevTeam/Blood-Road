using System.Collections.Generic;
using Characters.Information.Structs;
using UnityEngine;

namespace Characters.Information
{
    public class StatesInfo : MonoBehaviour
    {
        [SerializeField] private CurrentStateInfo[] currentStatesInfo;
        private Dictionary<string, StateInfo> _states;
        public StateInfo GetState(string nameState) => _states[nameState];

        private void Awake()
        {
            _states = new Dictionary<string, StateInfo>();
            foreach (var state in currentStatesInfo)
            {
                _states.Add(state.StateName, state.StateInfo);
            }
        } 
    }
}