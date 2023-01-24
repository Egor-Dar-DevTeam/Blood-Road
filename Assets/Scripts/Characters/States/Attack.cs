using System.Threading.Tasks;
using Characters.Animations;
using Characters.Information.Structs;
using UnityEngine;

namespace Characters.Player.States
{
    public class Attack : BaseState
    {
        protected IInteractable _interactable;
        protected bool _setDamage;
        private bool _isPlayer;
        protected int _damage;
        private TransitionAndStatesData transitionAndStatesData;
        private int _currentMilliseconds;
        public bool CanSkip { get; private set; }
        public int Milliseconds => _currentMilliseconds;

        public Attack(IAnimationCommand animation, StateInfo statesInfo, int damage, bool isPlayer, TransitionAndStatesData data, VFXTransforms vfxTransforms) : base(
            animation, statesInfo,vfxTransforms)
        {
            _damage = damage;
            _isPlayer = isPlayer;
            _parameterName = "attack";
            transitionAndStatesData = data;
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
                int milliseconds = SecondToMilliseconds(_animation.LengthAnimation(_parameterName) / 2);
                _currentMilliseconds = milliseconds*2;
                await Task.Delay(milliseconds);
                _currentMilliseconds = milliseconds;
                transitionAndStatesData.CharacterData.UseEnergy();
                transitionAndStatesData.EnergyEvent?.Invoke(transitionAndStatesData.CharacterData.Energy);
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
            _setDamage = false;
        }
    }
}