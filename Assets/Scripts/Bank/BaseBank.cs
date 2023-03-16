using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Banks
{
    public delegate void Add(int value);

    public delegate void Remove(int value);

    public delegate int GetValue();

    public abstract class BaseBank
    {
        protected string NameBank;
        private int _value;
        private Add _add;
        private Remove _remove;
        private GetValue getValue;
        public BankDelegates Delegates => new BankDelegates(_add = Add, _remove = Remove, getValue = (() => _value));

        public virtual void Initialize(string name)
        {
            _value = PlayerPrefs.GetInt(NameBank);
        }


        private void Add(int count)
        {
            _value = Mathf.Clamp(_value += count, 0, Int32.MaxValue);
            PlayerPrefs.SetInt(NameBank, _value);
        }

        private void Remove(int count)
        {
            _value = Mathf.Clamp(_value -= count, 0, Int32.MaxValue);
            PlayerPrefs.SetInt(NameBank, _value);
        }
    }

    public struct BankDelegates
    {
        public Add Add { get; }
        public Remove Remove { get; }

        public int Value
        {
            get => _getValue.Invoke();
        }

        private GetValue _getValue;

        public BankDelegates(Add add, Remove remove, GetValue getValue)
        {
            _getValue = getValue;
            Add = add;
            Remove = remove;
        }
    }
}