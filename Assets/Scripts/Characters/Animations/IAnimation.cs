using UnityEngine;

namespace Characters
{
    public interface IAnimation
    {
        void Initialize(Animator animator);
        void Idle();
        void Run();
        void Attack();
        void Aggressive();
        void Neutral();
        void Taking();
        void Die();
        void Shield();
    }
}