using System;
using UnityEngine;

namespace Characters
{
    [Serializable]
    public struct AnimationsCharacterData
    {
        [SerializeField] private AnimationClip idle;
        [SerializeField] private AnimationClip run;
        [SerializeField] private AnimationClip attack;
        [SerializeField] private AnimationClip die;
        [SerializeField] private AnimationClip shield;

        public AnimationClip Idle => idle;
        public AnimationClip Run => run;
        public AnimationClip Attack => attack;
        public AnimationClip Die => die;
        public AnimationClip Shield => shield;
        
    }
}