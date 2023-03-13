using System;
using Characters.Animations;
using Characters.Information.Structs;
using JetBrains.Annotations;
using UnityEngine;

namespace Characters.Player.States
{
    public class RunToPoint : BaseState
    {
        private readonly Transform _transform;
        private Transform _point;
        private CharacterController _characterController;
        private float _speed;
        private float _stopingDistance;

        public RunToPoint(IAnimationCommand animation, RunToPointData data, StateInfo stateInfo,
            VFXTransforms vfxTransforms) : base(animation, stateInfo, vfxTransforms)
        {
            _characterController = data.CharacterController;
            _transform = data.ThisCharacter;
            _speed = data.Speed;
            _stopingDistance = data.StopDistance;
            _parameterName = "run";
        }

        public void SetPoint([CanBeNull] Transform point)
        {
            _point = point;
        }

        public override void Enter()
        {
            base.Enter();
            _animation.SetAnimation(_parameterName);
            _characterController.enabled = true;
        }

        public override void Tick(float tickTime)
        {
            if (_point == null) return;
            if (Vector3.Distance(_point.position, _transform.position) >= _stopingDistance)
            {
                var directionMove = _transform.forward;
                directionMove.y = -0.9f;
                _characterController.Move(directionMove * (_speed * tickTime));
            }
        }

        public override void Exit()
        {
            _characterController.enabled = false;
        }
    }

    [Serializable]
    public struct RunToPointData
    {
        public CharacterController CharacterController;
        public float Speed;
        public float StopDistance;
        [HideInInspector] public Transform ThisCharacter;
    }
}