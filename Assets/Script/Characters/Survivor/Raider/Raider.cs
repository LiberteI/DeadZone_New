using System;
using UnityEngine;
using System.Collections.Generic;

public enum RaiderStateType
{
    Idle,
    Walk,
    Run,
    Jump,
    Hurt,
    Die,
    Reload,
    Melee,
    Shoot
}

[Serializable]
public class RaiderParameter : SurvivorParameters
{
    public BaseMovementManager movementManager;

    public RaiderShootingManager shootingManager;

    public RaiderMeleeManager meleeManager;

    public SurvivorAI aiControl;

    public Transform standMuzzle;

    public bool isCrouching;

    public bool isShooting;

    public bool isDoingMelee;

    public bool isReloading;
}


public class Raider : SurvivorBase
{
    private Dictionary<RaiderStateType, SurvivorIState> states = new Dictionary<RaiderStateType, SurvivorIState>();

    public new RaiderParameter parameter;

    void Awake()
    {
        base.parameter = parameter;

        SurvivorManager.survivorList.Add(parameter.survivorContainer);
    }

    void Start()
    {
        states.Add(RaiderStateType.Idle, new RaiderIdleState(this));

        states.Add(RaiderStateType.Walk, new RaiderWalkState(this));

        states.Add(RaiderStateType.Run, new RaiderRunState(this));

        states.Add(RaiderStateType.Jump, new RaiderJumpState(this));

        states.Add(RaiderStateType.Hurt, new RaiderHurtState(this));

        states.Add(RaiderStateType.Melee, new RaiderMeleeState(this));

        states.Add(RaiderStateType.Die, new RaiderDieState(this));

        states.Add(RaiderStateType.Shoot, new RaiderShootState(this));

        states.Add(RaiderStateType.Reload, new RaiderReloadState(this));

        TransitionState(RaiderStateType.Idle);
    }

    void Update()
    {
        SynchroniseFaceDir();
        // Debug.Log(parameter);
        base.currentState.OnUpdate();

        TryStartShooting();

        if (!isPlayedByPlayer)
        {
            return;
        }
        base.currentState.HandleInput();
    }

    public void TransitionState(RaiderStateType newState)
    {
        // exit current state
        // Debug.Log($"Transition from {currentState} to {newState}");
        if (base.currentState != null)
        {
            base.currentState.OnExit();
        }

        // assign new state from dictionary
        base.currentState = states[newState];

        // execute OnEnter once;
        base.currentState.OnEnter();
    }
    public void TryStartShooting()
    {
        if (isPlayedByPlayer)
        {
            return;
        }
        if (!parameter.shootingManager.CanShoot())
        {
            return;
        }
        if (parameter.aiControl.shouldShoot)
        {
            TransitionState(RaiderStateType.Shoot);
        }
    }
    private void SynchroniseFaceDir()
    {
        if (transform.rotation == Quaternion.Euler(0, 180f, 0))
        {
            parameter.isFacingRight = false;
        }
        else if (transform.rotation == Quaternion.Euler(0, 0, 0))
        {
            parameter.isFacingRight = true;
        }
    }
}
