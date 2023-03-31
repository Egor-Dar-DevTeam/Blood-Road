using Characters;
using Characters.Information;
using Characters.Player;
using Characters.Player.States;
using Dreamteck.Splines;
using Interaction;
using JetBrains.Annotations;
using UnityEngine;
using Attack = Characters.Player.States.Attack;

public struct TransitionAndStatesData
{
    public Animator Animator { get; }

    public GetCurrentPoint GetCurrentPoint { get; }

    public Transform Transform { get; }

    public RunToPointData RunToPointData { get; }
    public Money MoneyPrefab { get; }

    public StatesInfo StatesInfo { get; }
    public VFXTransforms VFXTransforms { get; }
    public AnimatorOverrideController AnimatorOverrideController { get; }
    public CharacterController CharacterController { get; }
    public SplineFollower SplineFollower { get; }

    [CanBeNull] public GetIsAttack GetIsAttack { get; }

    [CanBeNull] public CharacterData CharacterData { get; }
    [CanBeNull] public GetRecoil GetRecoil { get; }

    public Attack Attack { get; private set; }
    public Die Die { get; private set; }

    public HasCharacter HasCharacter { get; }

    public int Damage { get; }

    public TransitionAndStatesData(Animator animator, GetCurrentPoint getCurrentPoint, Transform transform,
        RunToPointData runToPointData, [CanBeNull] GetIsAttack getIsAttack, [CanBeNull] CharacterData characterData, [CanBeNull] GetRecoil getRecoil,
        StatesInfo statesInfo, int damage, HasCharacter hasCharacter, AnimatorOverrideController animatorOverrideController,VFXTransforms vfxTransforms,
        [CanBeNull] SplineFollower splineFollower=null, [CanBeNull] Money moneyPrefab=null)
    {
        Animator = animator;
        GetCurrentPoint = getCurrentPoint;
        Transform = transform;
        RunToPointData = runToPointData;
        GetIsAttack = getIsAttack;
        CharacterData = characterData;
        GetRecoil = getRecoil;
        StatesInfo = statesInfo;
        Attack = null;
        Die = null;
        Damage = damage;
        HasCharacter = hasCharacter;
        CharacterController = runToPointData.CharacterController;
        AnimatorOverrideController = animatorOverrideController;
        VFXTransforms = vfxTransforms;
        SplineFollower = splineFollower;
        MoneyPrefab = moneyPrefab;
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