using System;
using System.Collections.Generic;
using UnityEngine;

/*
    Tank's chest is busted open with tusks. It has the highest health volume and the way it attacks is to swing fist or grab survivor with its tusks.

    Tank responds to first couple of hits and then gets 5+ seconds' super armor, when it does not respond but velocity gets nerfed.

    Tank's fist swing cause huge damage and knockback.

    Tank's tusk attack: control the survivor and use its tusk squeeze the survivor
*/
public enum TankStateType
{
    Walk, Die, TuskAttack, Attack, Hurt
}
[Serializable]
public class TankParameter : BaseZombieParameter
{
    
}
public class Tank : BaseZombie
{
    public new TankParameter parameter;

    private Dictionary<TankStateType, IState> states = new Dictionary<TankStateType, IState>();

    public int tankWorth;
    void OnEnable()
    {
        base.worth = tankWorth;

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
        states.Add(TankStateType.Walk, new TWalkState(this));

        states.Add(TankStateType.Die, new TDieState(this));

        states.Add(TankStateType.Hurt, new THurtState(this));

        states.Add(TankStateType.TuskAttack, new TTuskAttackState(this));

        states.Add(TankStateType.Attack, new TAttackState(this));

        TransitionState(TankStateType.Walk);
    }

    public void TransitionState(TankStateType newState)
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

        TransitionState(TankStateType.Die);

    }
}
