using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Characters.Information.Structs
{
    [Serializable]
    public struct CurrentStateInfo
    {
        [SerializeField] private string stateName;
        [SerializeField] private StateInfo stateInfo;
        public string StateName => stateName;
        public StateInfo StateInfo => stateInfo;
    }
}