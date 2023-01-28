namespace Characters.MapperSystem
{
    public interface IMapper<TKey, TValue>
    {
        public void AddValue(TKey key, TValue value);
        public void RemoveByKey(TKey key);
        public TValue GetValue(TKey key);
    }
}