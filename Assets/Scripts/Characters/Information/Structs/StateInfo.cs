using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Characters.Information.Structs
{
    [Serializable]
    public struct StateInfo
    {
        [SerializeField] private AnimationClip clip;
        [SerializeField] [CanBeNull] private VFXEffect vfxEffect;
        public AnimationClip Clip => clip;
        [CanBeNull] public VFXEffect VFXEffect => vfxEffect;
    }
}