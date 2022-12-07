using System;

namespace Characters
{
    public interface IInit<in T> where T: Delegate
    {
        public void Initialize(T setPointDelegate);
    }
}