using System;
using System.Collections.Generic;
using Characters.Player;

namespace Characters.EffectSystem
{
    [Serializable]
    public class Applyer
    {
        private CharacterData _characterData;
        public void RechangeCharacterDataValues(EffectData effectData)
        {
            _characterData.Damaged(effectData.HealthDamage);
        }

        public void Initialize(CharacterData data)
        {
            _characterData = data;
        }
    }
}