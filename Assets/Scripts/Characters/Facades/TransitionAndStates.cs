using Better.UnityPatterns.Runtime.StateMachine;
using Better.UnityPatterns.Runtime.StateMachine.States;
using Characters.Animations;
using Characters.Player;
using Characters.Player.States;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;

namespace Characters.Facades
{
    public abstract class TransitionAndStates
    {
        protected IRunCommand _animation;

        protected Run _runState;
        protected Idle _idleState;
        protected Attack _attackState;
        protected Shield _shieldState;
        protected Die _dieState;
        protected StateMachine<BaseState> _stateMachine;
        protected CharacterData characterData;
        protected event GetIsAttack GetIsAttack;
        protected event GetCurrentPoint CurrentPoint;
        private CapsuleCollider _capsuleCollider;
        private event HasCharacter _hasCharacter;
        private DieDelegate _dieDelegate;
        private bool _isDeath;

        public DieDelegate DieDelegate => _dieDelegate;

        public virtual void Initialize(TASData data)
        {
            _dieDelegate = Death;
            _capsuleCollider = data.CapsuleCollider;
            characterData = data.CharacterData;
            if (data.GetIsAttack != null)
            {
                GetIsAttack += data.GetIsAttack;
            }

            _hasCharacter += data.HasCharacter;


            CurrentPoint += data.GetCurrentPoint;
        }

        protected virtual void StatesInit(Animator animator, NavMeshAgent agent)
        {
            _animation = new AnimatorController(animator);

            _runState = new Run(_animation, agent);
            _idleState = new Idle(_animation);
            _shieldState = new Shield(_animation, agent);
            _stateMachine = new StateMachine<BaseState>();
        }

        public void Destroy()
        {
            _stateMachine.ChangeState(_idleState);
        }

        protected virtual void TransitionInit(Transform transform, NavMeshAgent agent)
        {
            _stateMachine.AddTransition(_dieState, () => _isDeath);
        }

        protected virtual bool isRuning(Transform transform, NavMeshAgent agent)
        {
            if (CurrentPoint?.Invoke() != null)
            {
                _runState.SetPoint(CurrentPoint?.Invoke().GetObject());
                var position = CurrentPoint?.Invoke().GetObject().position;
                if (position != null &&
                    Vector3.Distance(transform.position, (Vector3)position) >= agent.stoppingDistance)
                {
                    return true;
                }

                return false;
            }
            else
            {
                return false;
            }
        }

        public void Update()
        {
            _stateMachine.Tick(Time.deltaTime);
        }

        protected IInteractable GetCurrentPoint()
        {
            return CurrentPoint?.Invoke();
        }

        protected bool IsAttack()
        {
            return GetIsAttack.Invoke();
        }

        private void Death()
        {
            _isDeath = true;
        }
    }
}