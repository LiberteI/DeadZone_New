using UnityEngine;
using System;
using System.Collections.Generic;
/*
    Boomer used to be a scientist working in lab. After getting infected, the virus gets trapped inside its suit. Boomer is extremely fragile. Getting hit, it will explode and its
        remain liquid will cause other infected to switch aggro.
*/

/*
    Boomer starts Idling. Then Transition to Walk, if survivor is in range, attack

    Getting hurt, right after hurt animation, explode(using particle effect)
*/
public enum BoomerStateType
{
    Idle, Walk, Hurt, Attack, Explode
}
[Serializable]
public class BoomerParameter : BaseZombieParameter
{
    public float explosionRange;
}
public class Boomer : BaseZombie
{
    public new BoomerParameter parameter;

    private Dictionary<BoomerStateType, IState> states = new Dictionary<BoomerStateType, IState>();

    public int boomerWorth;
    void OnEnable()
    {
        base.worth = boomerWorth;

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
        states.Add(BoomerStateType.Idle, new BIdleState(this));

        states.Add(BoomerStateType.Explode, new BExplodeState(this));

        states.Add(BoomerStateType.Hurt, new BHurtState(this));

        states.Add(BoomerStateType.Attack, new BAttackState(this));

        states.Add(BoomerStateType.Walk, new BWalkState(this));

        TransitionState(BoomerStateType.Idle);
    }

    public void TransitionState(BoomerStateType newState)
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

        TransitionState(BoomerStateType.Explode);

    }
}
