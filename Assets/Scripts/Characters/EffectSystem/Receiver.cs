using System;
using System.Collections.Generic;
using Characters.Facades;
using Characters.Player;

namespace Characters.EffectSystem
{
    [Serializable]
    public class Receiver
    {
        private IAnimatableEffect _animatable;
        private EffectData _effectData;
        private Applyer _applyer;
        public void Initialize(IAnimatableEffect animatable, Applyer applyer)
        {
            _animatable = animatable;
            _applyer = applyer;
        }

        public void RegisterAnimator(Type type)
        {
            _animatable.SetCurrentEffectID(type);
            _applyer.RechangeCharacterDataValues(_effectData);
        }

        public void Receive(EffectData data)
        {
            _effectData = data;
        }
    }
}