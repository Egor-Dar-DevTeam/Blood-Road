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
        private Rigidbody _rb;
        private float _speed;
        private float _stopingDistance;
        public Transform Point => _point;

        public RunToPoint(IAnimationCommand animation, RunToPointData data, StateInfo stateInfo,
            VFXTransforms vfxTransforms) : base(animation, stateInfo, vfxTransforms)
        {
            _rb = data.Rigidbody;
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
        }

        public override void Tick(float tickTime)
        {
            if (_point == null) return;
            if (Vector3.Distance(_point.position, _transform.position) >= _stopingDistance)
            {
                _rb.velocity = _transform.forward * _speed;
            }
        }

        public override void Exit()
        {
        }
    }

    [Serializable]
    public struct RunToPointData
    {
        public Rigidbody Rigidbody;
        public float Speed;
        public float StopDistance;
        [HideInInspector] public Transform ThisCharacter;
    }
}