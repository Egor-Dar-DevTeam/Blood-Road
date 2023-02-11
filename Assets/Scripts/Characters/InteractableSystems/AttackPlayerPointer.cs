using System;
using System.Threading.Tasks;
using Characters.Information;
using Characters.Information.Structs;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Characters.InteractableSystems
{
    public delegate void Attack(StateInfo info);

    public delegate void SetAttackSpeed(float value);

    public class AttackPlayerPointer : MonoBehaviour, IInit<Attack>, IInit<SetAttackSpeed>
    {
        [SerializeField] private StateInfo[] attackVariants;
        private bool _isWait;
        private int _clickCount;
        private event Attack _attack;
        private event SetAttackSpeed _setAttackSpeed;

        private void Start()
        {
            Wait();
        }

        private void Update()
        {
            if (!Input.GetMouseButtonDown(0)) return;
            _clickCount++;
            SetAttackSpeed(_clickCount);
        }

        private void SetAttackSpeed(int countClick)
        {
            switch (countClick)
            {
                case >= 2 and <= 4:
                    _setAttackSpeed?.Invoke(2);
                    break;
                case >= 5 and <= 8:
                    _setAttackSpeed?.Invoke(3);
                    break;
            }
            _attack?.Invoke(attackVariants[Random.Range(0,attackVariants.Length)]);
        }

        private async void Wait()
        {
            for (int i = 0;;)
            {
                //_clickCount++;
                //  _isWait = true;
                await Task.Delay(1000);
                // _isWait = false;
                _clickCount = 1;
                _setAttackSpeed?.Invoke(_clickCount);
            }
        }

        public void Initialize(Attack subscriber)
        {
            _attack += subscriber;
        }


        public void Initialize(SetAttackSpeed subscriber)
        {
            _setAttackSpeed += subscriber;
        }
    }
}