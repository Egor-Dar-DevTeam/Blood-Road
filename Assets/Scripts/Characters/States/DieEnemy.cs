using System.Threading.Tasks;
using Characters.Animations;
using Characters.Information.Structs;
using DG.Tweening;
using UnityEngine;

namespace Characters.Player.States
{
    public class DieEnemy : Die
    {
        private Money _moneyPrefab;
        private Transform _player;
        public DieEnemy(IAnimationCommand animation, StateInfo stateInfo, CapsuleCollider capsuleCollider, Rigidbody rigidbody,VFXTransforms vfxTransforms) : base(
            animation, stateInfo, capsuleCollider, rigidbody,vfxTransforms)
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

        private const int SECONDS = 1000;

        public static int SecondToMilliseconds(float second)
        {
            var result = Mathf.RoundToInt(second * SECONDS);
            return result;
        }

        private async void DieTime()
        {
            await Task.Delay(SecondToMilliseconds(_animation.LengthAnimation(_parameterName) / 3));
            var money = Object.Instantiate(_moneyPrefab, _vfxTransforms.Center.position,Quaternion.identity);
            money.SetPlayer(_player);
            await Task.Delay(SecondToMilliseconds(_animation.LengthAnimation(_parameterName)));
            if(_capsuleCollider!=null) _capsuleCollider.gameObject.transform.DOMoveY(_capsuleCollider.transform.position.y - 2, 10f)
                .OnComplete((() =>
                {
                    Object.Destroy(_capsuleCollider.gameObject);
                }));
        }
    }
}