using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Threading;
/*
    This script is the cross road linking Managers and parameters.
*/
public enum SWATStateType{
    Idle,
    Walk,
    Run,
    Hurt,
    Die,
    StandShoot,
    CrouchShoot,
    Melee,
    Reload,
    Jump,
    Crouch
}
[Serializable]
public class SWATParameter : SurvivorParameters
{

    public SWATMovementManager movementManager;

    public SWATShootingManager shootingManager;

    public SurvivorAI aiControl;

    public Transform standMuzzle;

    public Transform crouchMuzzle;

    // universal bool
    public bool isShooting;

    public bool isReloading;

    public bool isDoingMelee;

    public bool isCrouching;
    
}


public class SWAT : SurvivorBase
{
    private Dictionary<SWATStateType, SurvivorIState> states = new Dictionary<SWATStateType, SurvivorIState>();

    public new SWATParameter parameter;

    

    // assign parameter at Awake
    void Awake()
    {
        base.parameter = parameter;

        SurvivorManager.survivorList.Add(parameter.survivorContainer);

    }

    void Start()
    {
        // add states into the dictionary, taking a parameter of Player script
        states.Add(SWATStateType.Idle, new SWATIdleState(this));
        states.Add(SWATStateType.Walk, new SWATWalkState(this));
        states.Add(SWATStateType.Run, new SWATRunState(this));
        states.Add(SWATStateType.Hurt, new SWATHurtState(this));
        states.Add(SWATStateType.Die, new SWATDieState(this));
        states.Add(SWATStateType.StandShoot, new SWATStandShootState(this));
        states.Add(SWATStateType.CrouchShoot, new SWATCrouchShootState(this));
        states.Add(SWATStateType.Melee, new SWATMeleeState(this));
        states.Add(SWATStateType.Reload, new SWATReloadState(this));
        states.Add(SWATStateType.Jump, new SWATJumpState(this));
        states.Add(SWATStateType.Crouch, new SWATCrouchState(this));

        base.currentState = states[SWATStateType.Idle];
        // Debug.Log("should added");
    }
    public void TransitionState(SWATStateType newState)
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

    

    public void TryStartShooting()
    {
        if (isPlayedByPlayer)
        {
            return;
        }
        if (parameter.aiControl.shouldShoot)
        {
            TransitionState(SWATStateType.StandShoot);
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
