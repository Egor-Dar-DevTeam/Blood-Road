using Banks;

namespace Characters.BottlesSystem
{
    public sealed class Bottle : BaseBank
    {
        public override void Initialize(string name)
        {
            NameBank = "Bottle" + name;
            base.Initialize(name);
        }
    }
}