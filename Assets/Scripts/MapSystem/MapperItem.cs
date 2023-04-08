using UnityEngine;

namespace MapSystem
{
    public abstract class MapperItem : ScriptableObject
    {
        public abstract void Map(MappersMaped mappers);
    }
}