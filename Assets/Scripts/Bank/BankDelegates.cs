using Characters.InteractableSystems;

namespace Banks
{
    public struct BankDelegates
    {
        public Add Add { get; }
        public Remove Remove { get; }

        public IInit<GetValue> InitGetValue { get; }

        public BankDelegates(Add add, Remove remove, IInit<GetValue> getValue)
        {
            InitGetValue = getValue;
            Add = add;
            Remove = remove;
        }
    }
}