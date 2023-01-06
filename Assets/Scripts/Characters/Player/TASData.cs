using Characters;
using Characters.Player;
using Characters.Player.States;
using JetBrains.Annotations;
using UI.CombatHUD;
using UnityEngine;
using UnityEngine.AI;

public struct TASData
{
    public Animator Animator { get; }

    public GetCurrentPoint GetCurrentPoint { get; }

    public Transform Transform { get; }

    public NavMeshAgent NavMeshAgent { get; }

    public AnimationsCharacterData AnimationsCharacterData { get; }
    public AnimatorOverrideController AnimatorOverrideController { get; }
    public CapsuleCollider CapsuleCollider { get; }

    [CanBeNull] public GetIsAttack GetIsAttack { get; }

    [CanBeNull] public CharacterData CharacterData { get; }

    public UpdateEnergyDelegate EnergyEvent;

    public Attack Attack { get; private set; }
    public Die Die { get; private set; }

    public HasCharacter HasCharacter { get; }

    public int Damage { get; }

    public TASData(Animator animator, GetCurrentPoint getCurrentPoint, Transform transform,
        NavMeshAgent agent, [CanBeNull] GetIsAttack getIsAttack, [CanBeNull] CharacterData characterData,
        AnimationsCharacterData animationsCharacterData, int damage, HasCharacter hasCharacter,
        CapsuleCollider capsuleCollider, AnimatorOverrideController animatorOverrideController,
        [CanBeNull] UpdateEnergyDelegate updateEnergyDelegate = null)
    {
        Animator = animator;
        GetCurrentPoint = getCurrentPoint;
        Transform = transform;
        NavMeshAgent = agent;
        GetIsAttack = getIsAttack;
        CharacterData = characterData;
        AnimationsCharacterData = animationsCharacterData;
        Attack = null;
        Die = null;
        Damage = damage;
        HasCharacter = hasCharacter;
        CapsuleCollider = capsuleCollider;
        AnimatorOverrideController = animatorOverrideController;
        EnergyEvent = updateEnergyDelegate;
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