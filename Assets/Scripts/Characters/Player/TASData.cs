using Characters.Player;
using Characters.Player.States;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;

public struct TASData
{
    public Animator Animator { get; }

    public GetCurrentPoint GetCurrentPoint { get; }

    public Transform Transform { get; }

    public NavMeshAgent NavMeshAgent { get; }

    public AnimationClip AttackAnimationClip { get; }
    public AnimationClip DieAnimationClip { get; }
    public CapsuleCollider CapsuleCollider { get; }

    [CanBeNull]
    public GetIsAttack GetIsAttack { get; }

    [CanBeNull]
    public CharacterData CharacterData { get; }

    public Attack Attack { get; private set; }
    public Die Die { get; private set; }
    
    public HasCharacter HasCharacter { get; }

    public int Damage { get; }

    public TASData(Animator animator, GetCurrentPoint getCurrentPoint, Transform transform,
        NavMeshAgent agent, [CanBeNull] GetIsAttack getIsAttack, [CanBeNull] CharacterData characterData,
        AnimationClip attackAnimationClip, AnimationClip dieAnimationClip, int damage, HasCharacter hasCharacter, CapsuleCollider capsuleCollider)
    {
        Animator = animator;
        GetCurrentPoint = getCurrentPoint;
        Transform = transform;
        NavMeshAgent = agent;
        GetIsAttack = getIsAttack;
        CharacterData = characterData;
        AttackAnimationClip = attackAnimationClip;
        DieAnimationClip = dieAnimationClip;
        Attack = null;
        Die = null;
        Damage = damage;
        HasCharacter = hasCharacter;
        CapsuleCollider = capsuleCollider;
    }

    public void CreateAttack(Attack attack)
    {
        Attack = attack;
    }

    public void CreateDie(Die die)
    {
        Die = die;
    }
}