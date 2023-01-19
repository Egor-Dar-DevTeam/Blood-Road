using Characters.Player.States;
using UnityEngine;
using UnityEngine.AI;

namespace Characters.Facades
{
    public class PlayerTransition : TransitionAndStates
    {
        public override void Initialize(TransitionAndStatesData data)
        {
            base.Initialize(data);
            StatesInit(data.Animator, data.NavMeshAgent, data.AnimatorOverrideController, data.VFXTransforms);
            data.CreateAttack(new Attack(_animation, _statesInfo.GetState("attack"), data.Damage, true, data, data.VFXTransforms));
            data.CreateDie( new Die(_animation,_statesInfo.GetState("die"), data.CapsuleCollider, data.VFXTransforms));
            _attackState = data.Attack;
            _dieState = data.Die;

            TransitionInit(data.Transform, data.NavMeshAgent);

        }

        protected override void TransitionInit(Transform transform, NavMeshAgent agent)
        {
            base.TransitionInit(transform, agent);
            //    _stateMachine.AddTransition( _damagedState, _idleState,()=>true);
            _stateMachine.AddTransition(_idleState, _runState, () =>isRuning(transform, agent));
            _stateMachine.AddTransition(_damagedState, _shieldState, () =>
            {
                return _damagedState.CanSkip&& Vector3.Distance(transform.position, GetCurrentPoint().GetObject().position) <=
                    agent.stoppingDistance+.3f;
            } );
            _stateMachine.AddTransition(_damagedState, _attackState, (() =>
            {
                _attackState.SetPoint(GetCurrentPoint());
                return IsAttack();
            }));

        _stateMachine.AddTransition(_idleState, _shieldState,
            () =>
            {
                var point = GetCurrentPoint();
                if (point==null)
                {
                    return false;
                }
                var objectPoint = point.GetObject();
              return  Vector3.Distance(transform.position, objectPoint.position) <=
                    agent.stoppingDistance + .3f;
            });
            _stateMachine.AddTransition(_runState, _idleState, () => GetCurrentPoint() == null);
            _stateMachine.AddTransition(_runState, _shieldState, () =>
                Vector3.Distance(transform.position, GetCurrentPoint().GetObject().position) <=
                agent.stoppingDistance+.1f);
            _stateMachine.AddTransition(_shieldState, _idleState, () => GetCurrentPoint() == null);
            _stateMachine.AddTransition(_shieldState, _runState, () => isRuning(transform, agent));
            _stateMachine.AddTransition(_attackState, _runState, () => _attackState.CanSkip&&isRuning(transform, agent));
            _stateMachine.AddTransition(_shieldState, _attackState, () =>
            {
                _attackState.SetPoint(GetCurrentPoint());
                return IsAttack();
            });
            _stateMachine.AddTransition(_attackState, _shieldState,() => _attackState.CanSkip&&!IsAttack() );
            _stateMachine.AddTransition(_attackState, _idleState, (() => _attackState.CanSkip&&GetCurrentPoint() == null));
            _stateMachine.ChangeState(_idleState);
        }

        protected override bool isRuning(Transform transform, NavMeshAgent agent)
        {
            if (GetCurrentPoint() != null)
            {
                _runState.SetPoint(GetCurrentPoint().GetObject());
                var position = GetCurrentPoint().GetObject().position;
                if (position != null &&
                    Vector3.Distance(transform.position, (Vector3)position) >= agent.stoppingDistance+.2f)
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
    }
}