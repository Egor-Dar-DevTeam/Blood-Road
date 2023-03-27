using System;
using Better.Attributes.Runtime.Select;
using UnityEngine;

namespace Characters.Information.Structs
{
    [Serializable]
    public struct CurrentStateInfo
    {
        [SelectImplementation(typeof(BaseState))] [SerializeReference]
        private BaseState state;

        [SerializeField] private StateInfo stateInfo;
        public Type StateName => state.GetType();
        public StateInfo StateInfo => stateInfo;
    }
}