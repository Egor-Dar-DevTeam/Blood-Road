using System.Threading.Tasks;
using Characters.Animations;
using UnityEngine;

namespace Characters.Player.States
{
    public class Attack : BaseState
    {
        protected IInteractable _interactable;
        protected bool _setDamage;
        private bool _isPlayer;
        protected int _damage;
        private TASData _tasData;
        public bool CanSkip { get; private set; }

        public Attack(IRunCommand animation, AnimationClip clip, int damage, bool isPlayer, TASData data) : base(
            animation, clip)
        {
            _damage = damage;
            _isPlayer = isPlayer;
            _parameterName = "attack";
            _tasData = data;
        }

        public void SetPoint(IInteractable point)
        {
            _interactable = point;
        }

        public override void Enter()
        {
            CanSkip = false;
            base.Enter();
            _animation.SetAnimation(_parameterName);
            _setDamage = true;
            SetDamage();
        }

        private async void SetDamage()
        {
            do
            {
                var milliseconds = SecondToMilliseconds(_animation.LengthAnimation(_parameterName) / 2);
                await Task.Delay(milliseconds);
                _tasData.CharacterData.UseEnergy();
                _tasData.EnergyEvent?.Invoke(_tasData.CharacterData.Energy);
                _interactable.ReceiveDamage(_damage);
                await Task.Delay(milliseconds);
            } while (_setDamage && !_isPlayer);

            CanSkip = true;
        }


        public override void Tick(float tickTime)
        {
        }

        public override void Exit()
        {
            Debug.Log("Attack finish");
            _setDamage = false;
        }
    }
}