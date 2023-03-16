using System;


    public interface IInit<T> where T: Delegate
    {
        public void Initialize(T subscriber);
    }