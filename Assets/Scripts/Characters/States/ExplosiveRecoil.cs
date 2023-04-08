using System;
using System.Threading.Tasks;
using Characters.Animations;
using Characters.Information.Structs;
using MapSystem.Structs;
using UnityEngine;

namespace Characters.Player.States
{
    public class ExplosiveRecoil : BaseState
    {
        protected float _speed;
        protected float _radius;
        protected float _distance;
        protected Vector3 _direction;
        protected int _normalMilesecondsInDieAnim;
        protected CharacterController _controller;
        protected bool _stoodUp;

        public bool IsRecoiled => _distance >= _radius && _stoodUp;

        public ExplosiveRecoil(IAnimationCommand animation,  View view, VFXTransforms vfxTransforms,
            CharacterController characterController) : base(animation, view, vfxTransforms)
        {
            _parameterName = "die";
            _controller = characterController;
        }

        public override void Enter()
        {
            base.Enter();
            _distance = 0f;
            _stoodUp = false;
            _animation.SetAnimation(_parameterName);
            _animation.SetSpeedAnimation(1f);
            _normalMilesecondsInDieAnim = SecondToMilliseconds(_animation.LengthAnimation(_parameterName));
        }

        public void SetOrigin(Vector3 origin)
        {
            _direction = (_controller.transform.position - origin).normalized;
        }

        public void SetParameters(ExplosionParameters parameters)
        {
            _radius = parameters.Radius;
            _speed = parameters.Speed;
        }

        private async void StandUp()
        {
            await Task.Delay((int)(_normalMilesecondsInDieAnim * 0.75));
            _animation.SetSpeedAnimation(-1f);
            await Task.Delay(_normalMilesecondsInDieAnim);
            _stoodUp = true;
        }

        public override void Tick(float tickTime)
        {
            var offset = _speed * tickTime;
            if (_distance < _radius)
            {
                _controller.Move(_direction * _speed * tickTime);
                _distance += offset;

                if (_distance >= _radius)
                    StandUp();
            }
        }

        public override void Exit()
        {
        }
    }

    [Serializable]
    public struct ExplosionParameters
    {
        public float Speed;
        public float Radius;
    }
}
