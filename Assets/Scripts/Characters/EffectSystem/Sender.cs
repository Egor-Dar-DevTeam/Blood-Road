using System;
using System.Collections.Generic;
using Characters.AbilitiesSystem;
using Characters.LibrarySystem;
using Characters.WeaponSystem;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

namespace Characters.EffectSystem
{
    [Serializable]
    public class Sender
    {
        private EffectData _effectData;
        public void RegisterData(EffectData data)
        {
            _effectData=data;
        }

        public void RegisterReceiver(Receiver receiver)
        {
            receiver.Receive(_effectData);
        }
    }
}