using Characters.Player;
using Characters.Player.States;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;

public struct TASData
{
    private Animator _animator;
    private GetCurrentPoint _currentPointDelegate;
    private Transform _transform;
    private NavMeshAgent _agent;
    private AnimationClip _animationClip;
    private Attack _attack;
    [CanBeNull] private GetIsAttack _isAttackDelegate;
    [CanBeNull] private PlayerData _playerData;

    public Animator Animator => _animator;
    public GetCurrentPoint GetCurrentPoint => _currentPointDelegate;
    public Transform Transform => _transform;
    public NavMeshAgent NavMeshAgent => _agent;
    public AnimationClip AnimationClip => _animationClip;
    public GetIsAttack GetIsAttack => _isAttackDelegate;
    public PlayerData PlayerData => _playerData;
    public Attack Attack => _attack;

    public TASData(Animator animator, GetCurrentPoint getCurrentPoint, Transform transform,
        NavMeshAgent agent, [CanBeNull] GetIsAttack getIsAttack, [CanBeNull] PlayerData playerData,
        AnimationClip animationClip)
    {
        _animator = animator;
        _currentPointDelegate = getCurrentPoint;
        _transform = transform;
        _agent = agent;
        _isAttackDelegate = getIsAttack;
        _playerData = playerData;
        _animationClip = animationClip;
        _attack = null;
    }

    public void CreateAttack(Attack attack)
    {
        _attack = attack;
    }
}