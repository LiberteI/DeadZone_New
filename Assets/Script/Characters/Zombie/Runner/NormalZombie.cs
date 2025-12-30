using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

/*
    This script is a base for 4 normal zombies(Runner). They share the same behaviour but diffenrent sprite.
*/
[Serializable]
public class NormalZombieParameter : BaseZombieParameter{

}

public enum NormalZombieStateTypes{
    Attack, Hurt, Die, Idle, Walk
}
public class NormalZombie : BaseZombie
{
    public new NormalZombieParameter parameter;

    private Dictionary<NormalZombieStateTypes, IState> states = new Dictionary<NormalZombieStateTypes, IState>();

    public int runnerWorth = 5;
    void OnEnable()
    {
        base.worth = runnerWorth;

        EventManager.OnZombieDie += TransitionDieState;

        EventManager.OnLootCorpse += GetLooted;

        base.parameter = parameter;
    }

    void OnDisable()
    {
        EventManager.OnZombieDie -= TransitionDieState;

        EventManager.OnLootCorpse -= GetLooted;
    }

    void Start()
    {

        states.Add(NormalZombieStateTypes.Idle, new NZIdleState(this));

        states.Add(NormalZombieStateTypes.Walk, new NZWalkState(this));

        states.Add(NormalZombieStateTypes.Attack, new NZAttackState(this));

        states.Add(NormalZombieStateTypes.Hurt, new NZAttackState(this));

        states.Add(NormalZombieStateTypes.Die, new NZDieState(this));

        TransitionState(NormalZombieStateTypes.Idle);
    }

    public void TransitionState(NormalZombieStateTypes newState)
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

    private void TransitionDieState(GameObject zombie)
    {
        if (zombie != this.gameObject)
        {
            return;
        }

        TransitionState(NormalZombieStateTypes.Die);

    }

}
