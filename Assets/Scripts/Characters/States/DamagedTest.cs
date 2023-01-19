using System.Threading.Tasks;
using Characters.Animations;
using Characters.Information.Structs;
using UnityEngine;

namespace Characters.Player.States
{
    public class DamagedTest : BaseState
    {
        private int _milliseconds;
        private float _deltaTime;
        public bool CanSkip { get; private set; }
        public DamagedTest(IAnimationCommand animation, StateInfo stateInfo, VFXTransforms vfxTransforms) : base(
            animation, stateInfo, vfxTransforms)
        {
        }

        public void SetMilliseconds(int value)
        {
            _milliseconds = value;
        }

    public override void Enter()
    {
        CanSkip = false;
            Object.Instantiate(_vfxEffect, _vfxTransforms.Center);
            Object.Instantiate(_vfxEffect.gameObject, Vector3.zero, Quaternion.LookRotation(Vector3.left),_vfxTransforms.Center);
            Object.Instantiate(_vfxEffect.gameObject, Vector3.zero, Quaternion.LookRotation(Vector3.right),_vfxTransforms.Center);
            Wait();
        }

    private async void Wait()
    {
        await Task.Delay(_milliseconds-SecondToMilliseconds(_deltaTime));
        CanSkip = true;
    }

    public override void Tick(float tickTime)
    {
        _deltaTime = tickTime;
    }

        public override void Exit()
        {
           
        }
    }
}