using System;

namespace Characters.InteractableSystems
{
    public interface IInit<in T> where T: Delegate
    {
        public void Initialize(T subscriber);
    }
}