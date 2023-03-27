using System.Threading.Tasks;
using Characters.Information.Structs;
using Characters.InteractableSystems;
using UnityEngine;

namespace Characters.Player
{
    public delegate void Attack(StateInfo info);

    public delegate void SetAttackSpeed(float value);
    public class AttackVariants : MonoBehaviour,IInit<Attack>, IInit<SetAttackSpeed>
    {
        [SerializeField] private StateInfo[] attackVariants;
        private int _clickCount;
        private event Attack _attack;
        private event SetAttackSpeed _setAttackSpeed;

        private void Start()
        {
            Wait();
        }

        public void Attack(int index)
        {
            _clickCount++;
            switch (_clickCount)
            {
                case >= 2 and <= 4:
                    _setAttackSpeed?.Invoke(2);
                    break;
                case >= 5 and <= 8:
                    _setAttackSpeed?.Invoke(3);
                    break;
            }
            if(attackVariants.Length<=index) return;
            _attack?.Invoke(attackVariants[index]);

        }
        private async void Wait()
        {
            for (;;)
            {
                await Task.Delay(1000);
                _clickCount = 1;
                _setAttackSpeed?.Invoke(_clickCount);
            }
        }
        public void Subscribe(Attack subscriber)
        {
            _attack += subscriber;
        }

        public void Unsubscribe(Attack unsubscriber)
        {
            _attack -= unsubscriber;
        }

        public void Subscribe(SetAttackSpeed subscriber)
        {
            _setAttackSpeed += subscriber;
        }

        public void Unsubscribe(SetAttackSpeed unsubscriber)
        {
            _setAttackSpeed -= unsubscriber;
        }
    }
}