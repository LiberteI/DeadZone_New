
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

/*
    Screamer will use scream to summon lots of infected using random zombie spawn points

    scream will also provoke stalkers.

    screamer should only scream once a lifetime and then behave like a stalker

    screamer start idling, then walk towards survivor. After 5 - 10 seconds, scream.
    Then it also runs
*/
public enum ScreamerStateType
{
    Idle, Walk, Run, Attack, Hurt, Die, Jump, Scream
}
[Serializable]
public class ScreamerParameter : BaseZombieParameter
{
    public float runSpeed;

    public bool isAttacking;

    public bool hasScreamed;

    public GameObject screamerCentreObj;

    public bool isScreaming;
}
public class Screamer : BaseZombie
{
    // hides the base parameter
    public new ScreamerParameter parameter;

    private Dictionary<ScreamerStateType, IState> states = new Dictionary<ScreamerStateType, IState>();

    private AnimatorStateInfo info;

    [SerializeField] private float timer;

    public int screamerWorth;

    void OnEnable()
    {
        base.parameter = parameter;

        EventManager.OnScream += SetHasScreamed;

        base.worth = screamerWorth;

        EventManager.OnZombieDie += TransitionDieState;

        EventManager.OnLootCorpse += GetLooted;
    }
    void OnDisable()
    {
        EventManager.OnScream -= SetHasScreamed;

        EventManager.OnZombieDie -= TransitionDieState;

        EventManager.OnLootCorpse -= GetLooted;

    }
    void Start()
    {
        states.Add(ScreamerStateType.Walk, new SCWalkState(this));

        states.Add(ScreamerStateType.Run, new SCRunState(this));

        states.Add(ScreamerStateType.Die, new SCDieState(this));

        states.Add(ScreamerStateType.Hurt, new SCHurtState(this));

        states.Add(ScreamerStateType.Attack, new SCAttackState(this));

        states.Add(ScreamerStateType.Idle, new SCIdleState(this));

        states.Add(ScreamerStateType.Jump, new SCJumpState(this));

        states.Add(ScreamerStateType.Scream, new SCScreamState(this));

        TransitionState(ScreamerStateType.Idle);

        timer = UnityEngine.Random.Range(5f, 10f);


    }

    void Update()
    {
        if (isDead)
        {
            return;
        }
        TryScream();

        TryActivateScreamRange();

        currentState.OnUpdate();
    }

    public void TransitionState(ScreamerStateType newState)
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

    private void TryActivateScreamRange()
    {
        if (parameter.isScreaming)
        {
            parameter.screamerCentreObj.SetActive(true);
        }
        else
        {
            parameter.screamerCentreObj.SetActive(false);
        }
    }
    public void TryScream()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;

            return;
        }
        if (parameter.hasScreamed)
        {
            return;
        }
        parameter.hasScreamed = true;

        TransitionState(ScreamerStateType.Scream);

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

    private void SetHasScreamed(GameObject screamer)
    {
        if (screamer != this.gameObject)
        {
            return;
        }
        parameter.hasScreamed = true;
    }

    private void TransitionDieState(GameObject zombie)
    {
        if (zombie != this.gameObject)
        {
            return;
        }

        StopAllCoroutines();
        
        TransitionState(ScreamerStateType.Die);

    }
}
