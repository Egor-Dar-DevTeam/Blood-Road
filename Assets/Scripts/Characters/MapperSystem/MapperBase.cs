using System.Collections.Generic;

namespace Characters.MapperSystem
{
    public abstract class MapperBase<TKey, TValue> : IMapper<TKey, TValue>
    {
        protected Dictionary<TKey, TValue> _dictionary;
        public TValue GetValue(TKey id) => _dictionary[id];

        public MapperBase()
        {
            _dictionary = new Dictionary<TKey, TValue>();
        }

        public virtual void AddValue(TKey key, TValue value)
        {
            if (_dictionary.ContainsKey(key) && _dictionary.ContainsValue(value)) return;
            _dictionary.Add(key, value);
        }

        public void RemoveByKey(TKey key)
        {
            if (_dictionary.ContainsKey(key)) _dictionary.Remove(key);
        }
    }
}