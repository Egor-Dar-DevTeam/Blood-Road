using System;
using Better.Attributes.Runtime.Select;
using Characters;
using UnityEngine;

namespace MapSystem.Structs
{
    [Serializable]
    public struct View
    {
        [field: SerializeField] public int ID { get; private set; }

        [field: SerializeReference]
        [field: SelectImplementation(typeof(BaseState))]
        public BaseState State { get; private set; }

        [field: SerializeField] public AnimationClip Animation { get; private set; }
        [field: SerializeField] public VFXEffect Effect { get; private set; }

        public View(View view)
        {
            Animation = view.Animation;
            Effect = view.Effect;
            State = view.State;
            ID = view.ID;
        }

        public View(AnimationClip animation, VFXEffect effect, BaseState state, int id)
        {
            Animation = animation;
            Effect = effect;
            State = state;
            ID = id;
        }
    }
}