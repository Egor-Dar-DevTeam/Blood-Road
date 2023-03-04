using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Characters.AbilitiesSystem.Ability
{
    [CreateAssetMenu(menuName = "ScriptableObjects/AbilityData", fileName = "AbilityData", order = 4)]
    public class AbilitiesData : ScriptableObject
    {
       [SerializeField] private List<AbilitySO> abilitiesSo;
       public List<AbilitySO> GetCopy() => new List<AbilitySO>(abilitiesSo);
    }
}