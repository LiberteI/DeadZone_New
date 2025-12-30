using System;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public enum SniperStateType
{
    Idle,
    Walk,
    Run,
    Crouch,
    Jump,
    Hurt,
    Die,
    StandShoot,
    CrouchShoot,
    Melee,
    Reload,
    Throw

}

[Serializable]
public class SniperParameter : SurvivorParameters
{
    public BaseMovementManager movementManager;

    public SniperShootingManager shootingManager;

    public SurvivorAI aiControl;

    public Transform crouchMuzzle;

    public Transform standMuzzle;

    public bool isCrouching;

    public bool isShooting;

    public bool isDoingMelee;

    public bool isReloading;
}
public class Sniper : SurvivorBase
{
    private Dictionary<SniperStateType, SurvivorIState> states = new Dictionary<SniperStateType, SurvivorIState>();

    public new SniperParameter parameter;

    void Awake()
    {
        base.parameter = parameter;

        SurvivorManager.survivorList.Add(parameter.survivorContainer);

        parameter.movementManager = this.gameObject.GetComponent<BaseMovementManager>();

        parameter.shootingManager = this.gameObject.GetComponent<SniperShootingManager>();

        parameter.aiControl = this.gameObject.GetComponent<SurvivorAI>();
    }

    void Start()
    {
        states.Add(SniperStateType.Idle, new SniperIdleState(this));

        states.Add(SniperStateType.Walk, new SniperWalkState(this));

        states.Add(SniperStateType.Run, new SniperRunState(this));

        states.Add(SniperStateType.Crouch, new SniperCrouchState(this));

        states.Add(SniperStateType.Throw, new SniperThrowState(this));

        states.Add(SniperStateType.Reload, new SniperReloadState(this));

        states.Add(SniperStateType.Jump, new SniperJumpState(this));

        states.Add(SniperStateType.Hurt, new SniperHurtState(this));

        states.Add(SniperStateType.Die, new SniperDieState(this));

        states.Add(SniperStateType.Melee, new SniperMeleeState(this));

        states.Add(SniperStateType.StandShoot, new SniperStandShootState(this));

        states.Add(SniperStateType.CrouchShoot, new SniperCrouchShootState(this));

        TransitionState(SniperStateType.Idle);
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
    public void TransitionState(SniperStateType newState)
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
            TransitionState(SniperStateType.StandShoot);
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
