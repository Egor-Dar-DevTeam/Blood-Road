using System.Threading.Tasks;
using Characters.Information.Structs;
using Characters.InteractableSystems;
using UnityEngine;

namespace Characters.Player
{
    public delegate void Attack(StateInfo info);

    public delegate void SetAttackSpeed(float value);
    public delegate void OverrideAttack(StateInfo stateInfo, bool state);
    public class AttackVariants : MonoBehaviour,IInit<Attack>, IInit<SetAttackSpeed>
    {
        [SerializeField] private StateInfo[] attackVariants;
        private int _clickCount;
        private event Attack _attack;
        private event SetAttackSpeed _setAttackSpeed;
        protected bool _canOverride;
        protected StateInfo _overrided;


        private void Start()
        {
            Wait();
        }

        public void StartOverridedStateInfo(StateInfo stateInfo, bool start)
        {
            _overrided = stateInfo;
            _canOverride = start;
        }

        public void Attack(int index)
        {
            _clickCount++;
            switch (_clickCount)
            {
                case >= 2 and <= 4:
                    TrySetAttackSpeed(2f);
                    break;
                case >= 5 and <= 8:
                    TrySetAttackSpeed(3f);
                    break;
            }
            if(attackVariants.Length<=index) return;
            _attack?.Invoke(_canOverride ? _overrided : attackVariants[index]);
            if (_canOverride) _setAttackSpeed?.Invoke(_overrided.AnimationSpeed);
        }

        private void TrySetAttackSpeed(float speed)
        {
            _setAttackSpeed?.Invoke(_canOverride ? _overrided.AnimationSpeed : speed);
        }

        private async void Wait()
        {
            for (;;)
            {
                await Task.Delay(1000);
                _clickCount = 1;
                TrySetAttackSpeed(_clickCount);
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