using Characters;
using Characters.AbilitiesSystem;
using Characters.Information;
using Characters.Information.Structs;
using Characters.Player;
using Characters.Player.States;
using JetBrains.Annotations;
using UI.CombatHUD;
using UnityEngine;
using UnityEngine.AI;

public struct TransitionAndStatesData
{
    public Animator Animator { get; }

    public GetCurrentPoint GetCurrentPoint { get; }

    public Transform Transform { get; }

    public NavMeshAgent NavMeshAgent { get; }

    public StatesInfo StatesInfo { get; }
    public AbilitiesInfo AbilitiesInfo { get; }
    public VFXTransforms VFXTransforms { get; }
    public AnimatorOverrideController AnimatorOverrideController { get; }
    public CapsuleCollider CapsuleCollider { get; }

    [CanBeNull] public GetIsAttack GetIsAttack { get; }

    [CanBeNull] public CharacterData CharacterData { get; }

    public UpdateEnergyDelegate EnergyEvent;

    public Attack Attack { get; private set; }
    public Die Die { get; private set; }

    public HasCharacter HasCharacter { get; }

    public int Damage { get; }

    public TransitionAndStatesData(Animator animator, GetCurrentPoint getCurrentPoint, Transform transform,
        NavMeshAgent agent, [CanBeNull] GetIsAttack getIsAttack, [CanBeNull] CharacterData characterData,
        StatesInfo statesInfo, int damage, HasCharacter hasCharacter,
        CapsuleCollider capsuleCollider, AnimatorOverrideController animatorOverrideController,VFXTransforms vfxTransforms,
        [CanBeNull] UpdateEnergyDelegate updateEnergyDelegate = null, [CanBeNull] AbilitiesInfo abilitiesInfo=null)
    {
        Animator = animator;
        GetCurrentPoint = getCurrentPoint;
        Transform = transform;
        NavMeshAgent = agent;
        GetIsAttack = getIsAttack;
        CharacterData = characterData;
        StatesInfo = statesInfo;
        Attack = null;
        Die = null;
        Damage = damage;
        HasCharacter = hasCharacter;
        CapsuleCollider = capsuleCollider;
        AnimatorOverrideController = animatorOverrideController;
        EnergyEvent = updateEnergyDelegate;
        AbilitiesInfo = abilitiesInfo;
        VFXTransforms = vfxTransforms;
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