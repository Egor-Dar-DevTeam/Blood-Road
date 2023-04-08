using System.Collections.Generic;
using MapSystem.Structs;
using UnityEngine;

namespace MapSystem
{
    [CreateAssetMenu(fileName = "OneToManyItems", menuName = "Mapper/OneToManyItems")]
    public class OneToManyItems : MapperItem
    {
        [SerializeField] private Item one;
        [SerializeField] private List<Item> many;

        public override void Map(MappersMaped mappers)
        {
            var stateCharacterKey = new StateCharacterKey(one.View.ID, one.View.State?.GetType(),
                one.Ability.AbilityCommand?.GetType());
            mappers.MappingList.AddValue(stateCharacterKey, many);
            mappers.MappingView.AddValue(stateCharacterKey, one.View);
            mappers.MappingAbilityByStateKey.AddValue(stateCharacterKey, one.Ability);
            mappers.MappingUI.AddValue(stateCharacterKey, one.UIInfo);
            foreach (var item in many)
            {
                mappers.MappingView.AddValue(
                    new StateCharacterKey(item.View.ID, item.View.State?.GetType(),
                        item.Ability.AbilityCommand?.GetType()), item.View);
            }
        }
    }
}