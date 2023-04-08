using System.Collections.Generic;
using MapSystem.Mappers;
using MapSystem.Structs;
using UnityEngine;
using View = MapSystem.Structs.View;

namespace MapSystem
{
    [DefaultExecutionOrder(-2000)]
    public class Placeholder : MonoBehaviour
    {
        [SerializeField] private List<MapperItem> items;
        private MappingView _mappingView;
        private MappingUI _mappingUI;
        private MappingList _mappingList;
        private MappingAbilityByUIInfo _mappingAbilityByUIInfo;
        private MappingAbilityByStateKey _mappingAbilityByStateKey;

        public bool TryGetView(StateCharacterKey stateCharacterKey, out View view)
        {
            var result = _mappingView.TryGetValue(stateCharacterKey, out View value);
            view = new(value);
            return result;
        }

        public bool TryGetUIInfo(StateCharacterKey stateCharacterKey, out UIInfo infoUI)
        {
            var result = _mappingUI.TryGetValue(stateCharacterKey, out UIInfo value);
            infoUI = new(value);
            return result;
        }

        public bool TryGetList(StateCharacterKey stateCharacterKey, out List<Item> items)
        {
            var result = _mappingList.TryGetValue(stateCharacterKey, out List<Item> value);
            items = new List<Item>(value);
            return result;
        }

        public bool TryGetAbility(UIInfo uiInfo, out Ability ability)
        {
            var result = _mappingAbilityByUIInfo.TryGetValue(uiInfo, out Ability value);
            ability = new(value);
            return result;
        }

        public bool TryGetAbility(StateCharacterKey stateCharacterKey, out Ability ability)
        {
            var result = _mappingAbilityByStateKey.TryGetValue(stateCharacterKey, out Ability value);
            ability = new(value);
            return result;
        }


        public void Awake()
        {
            _mappingView = new MappingView();
            _mappingUI = new MappingUI();
            _mappingList = new MappingList();
            _mappingAbilityByUIInfo = new MappingAbilityByUIInfo();
            _mappingAbilityByStateKey = new MappingAbilityByStateKey();
            foreach (var item in items)
            {
                item.Map(
                    new(_mappingUI, _mappingView, _mappingAbilityByUIInfo, _mappingAbilityByStateKey, _mappingList));
            }
        }
    }
}