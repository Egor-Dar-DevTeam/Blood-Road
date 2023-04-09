using System;
using Characters.InteractableSystems;
using UnityEngine;

namespace Banks
{
    public delegate void Add(int value);

    public delegate void Remove(int value);

    public delegate void GetValue(int value);

    [Serializable]
    public abstract class BaseBank : IInit<GetValue>
    {
        protected string NameBank;
        private int _value;
        private event GetValue _getValue;
        public BankDelegates Delegates => new(Add, Remove, this);

        public virtual void Initialize(string name)
        {
            _value = PlayerPrefs.GetInt(NameBank);
        }


        private void Add(int count)
        {
            _value = Mathf.Clamp(_value += count, 0, 100);
            _getValue?.Invoke(_value);
            PlayerPrefs.SetInt(NameBank, _value);
        }

        private void Remove(int count)
        {
            _value = Mathf.Clamp(_value -= count, 0, Int32.MaxValue);
            _getValue?.Invoke(_value);
            PlayerPrefs.SetInt(NameBank, _value);
        }

        public void Subscribe(GetValue subscriber)
        {
            _getValue += subscriber;
            _getValue?.Invoke(_value);
        }

        public void Unsubscribe(GetValue unsubscriber)
        {
            _getValue -= unsubscriber;
        }
    }
}