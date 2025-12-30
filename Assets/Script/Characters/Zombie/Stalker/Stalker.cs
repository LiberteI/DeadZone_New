using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
/*
    Stalker is the same as Runner regarding infected level, but they have swifter speed and higher damage.

    Stalker starts Idling: randomly play idle 1 or 2, if 2 is not found, play default. *

    Find the nearest target using find script. Use a triggered collider to foresee target.
        If it sees target in the front, run.
    If it is provoked by Screamer: run.

    For attack, randomly do 1 / 3 attacks, with 20% of chance to do combo.



*/
public enum StalkerStateType
{
    Idle, Walk, Run, Attack, Hurt, Die, Jump
}

[Serializable]
public class StalkerParameter : BaseZombieParameter
{
    public bool hasBeenProvoked;

    public float runSpeed;

    public bool isAttacking;


}
public class Stalker : BaseZombie
{

    // hides the base parameter
    public new StalkerParameter parameter;

    private Dictionary<StalkerStateType, IState> states = new Dictionary<StalkerStateType, IState>();

    private AnimatorStateInfo info;

    public int stalkerWorth;
    void OnEnable()
    {
        base.worth = stalkerWorth;

        EventManager.OnZombieDie += TransitionDieState;

        EventManager.OnLootCorpse += GetLooted;

        EventManager.OnIsWithinScreamRange += SetProvoked;

        EventManager.OnProvoked += SetProvoked;

        base.parameter = parameter;
    }
    void OnDisable()
    {
        EventManager.OnIsWithinScreamRange -= SetProvoked;

        EventManager.OnProvoked -= SetProvoked;

        EventManager.OnZombieDie -= TransitionDieState;

        EventManager.OnLootCorpse -= GetLooted;
    }
    void Start()
    {
        states.Add(StalkerStateType.Walk, new SWalkState(this));

        states.Add(StalkerStateType.Run, new SRunState(this));

        states.Add(StalkerStateType.Die, new SDieState(this));

        states.Add(StalkerStateType.Hurt, new SHurtState(this));

        states.Add(StalkerStateType.Attack, new SAttackState(this));

        states.Add(StalkerStateType.Idle, new SIdleState(this));

        states.Add(StalkerStateType.Jump, new SJumpState(this));

        TransitionState(StalkerStateType.Idle);
    }

    public void TransitionState(StalkerStateType newState)
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
        StopAllCoroutines();
        
        TransitionState(StalkerStateType.Die);

    }

    public void PerformComboAttack()
    {
        StartCoroutine(ExecuteComboAttack());
    }

    private IEnumerator ExecuteComboAttack()
    {
        parameter.isAttacking = true;

        parameter.animator.Play("Attack1");

        yield return WaitForAnimation("Attack1");

        parameter.animator.Play("Attack2");

        yield return WaitForAnimation("Attack2");

        parameter.animator.Play("Attack3");

        yield return WaitForAnimation("Attack3");

        parameter.isAttacking = false;
    }

    private IEnumerator WaitForAnimation(string name)
    {
        while (true)
        {
            info = parameter.animator.GetCurrentAnimatorStateInfo(0);

            yield return null;

            if (info.IsName(name))
            {
                break;
            }
        }

        while (true)
        {
            info = parameter.animator.GetCurrentAnimatorStateInfo(0);

            yield return null;

            if (info.normalizedTime > 0.99f)
            {
                break;
            }
        }
    }
    private void SetProvoked(ProvocationData data)
    {
        if (data.initiator != this.gameObject)
        {
            return;
        }
        parameter.hasBeenProvoked = true;
    }

    private void SetProvoked(GameObject receiver)
    {
        if (receiver != this.gameObject)
        {
            return;
        }
        parameter.hasBeenProvoked = true;
    }
}

