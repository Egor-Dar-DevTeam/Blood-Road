using MapSystem.Structs;
using UnityEngine;

namespace MapSystem
{
    [CreateAssetMenu(fileName = "OneToOneItems", menuName = "Mapper/OneToOneItems")]
    public class OneToOneItem : MapperItem
    {
        [SerializeField] private Item item;

        public override void Map(MappersMaped mappers)
        {
            var stateCharacterKey = new StateCharacterKey(item.View.ID, item.View.State?.GetType(), item.Ability.AbilityCommand?.GetType());
            mappers.MappingUI.AddValue(stateCharacterKey, item.UIInfo);
            mappers.MappingView.AddValue(stateCharacterKey, item.View);
            mappers.MappingAbilityByStateKey.AddValue(stateCharacterKey,
                item.Ability);
        }
    }
}