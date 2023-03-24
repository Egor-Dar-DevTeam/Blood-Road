using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Characters.Information.Structs
{
    [Serializable]
    public struct StateInfo
    {
        [SerializeField] internal AnimationClip clip;
        [SerializeField] [CanBeNull] internal VFXEffect vfxEffect;
        public AnimationClip Clip => clip;
        [CanBeNull] public VFXEffect VFXEffect => vfxEffect;
        internal Transform vfxTransform;

        public StateInfo(VFXEffect vFXEffect, AnimationClip clip, int duration,
            [CanBeNull] Transform transform = null, VFXSpawnType spawn = VFXSpawnType.Default
            , float speed = 1)
        {
            this.clip = clip;
            vfxEffect = vFXEffect;
            vfxTransform = transform;
            SpawnType = spawn;
            _duration = duration;
            _animationSpeed = speed;
        }

        public static StateInfo empty = new StateInfo();
        public VFXSpawnType SpawnType { get; }
        private int _duration;
        private float _animationSpeed;
        public float AnimationSpeed { get => _animationSpeed == 0f ? 1f : _animationSpeed; }
        public int DurationInMileseconds { get => _duration == 0 ? 100 : _duration; }
    }

    public enum VFXSpawnType : short
    {
        Default,
        UniversalBlow
    }
}
