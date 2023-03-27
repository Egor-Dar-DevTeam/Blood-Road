using System.Threading.Tasks;
using Characters.Animations;
using Characters.Information.Structs;
using DG.Tweening;
using Interaction;
using UnityEngine;

namespace Characters.Player.States
{
    public class DieEnemy : Die
    {
        private Money _moneyPrefab;
        private Transform _player;
public DieEnemy(){}
        public DieEnemy(IAnimationCommand animation, StateInfo stateInfo, CharacterController characterController,
             VFXTransforms vfxTransforms) : base(
            animation, stateInfo, characterController, vfxTransforms)
        {
            _animation = animation;
        }

        public void SetMoneyPrefab(Money prefab)
        {
            _moneyPrefab = prefab;
        }

        public void SetPlayerTransform(Transform player)
        {
            _player = player;
        }

        public override void Enter()
        {
            base.Enter();
            DieTime();
        }

        private async void DieTime()
        {
            characterController.GetComponent<CapsuleCollider>().enabled = false;
            await Task.Delay(SecondToMilliseconds(_animation.LengthAnimation(_parameterName) / 3));
            if (_moneyPrefab != null)
            {
                var money = Object.Instantiate(_moneyPrefab, _vfxTransforms.Center.position, Quaternion.identity);
                money.SetPlayer(_player);
            }
            await Task.Delay(SecondToMilliseconds(_animation.LengthAnimation(_parameterName)));
                characterController.transform
                    .DOMoveY(characterController.transform.position.y - 2, 10f)
                    .OnComplete((() => { Object.Destroy(characterController.gameObject); }));
        }   
    }
}