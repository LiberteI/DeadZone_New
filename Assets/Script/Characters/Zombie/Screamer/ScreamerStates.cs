using UnityEngine;

public class SCIdleState : IState
{
    private Screamer screamer;

    private ScreamerParameter parameter;

    public SCIdleState(Screamer screamer)
    {
        this.screamer = screamer;

        this.parameter = screamer.parameter;
    }

    public void OnEnter()
    {
        parameter.movementManager.DisableLinearVelocity();

        parameter.animator.Play("Idle");
    }

    public void OnUpdate()
    {
        if (parameter.aggroManager.currentTarget == null)
        {
            return;
        }
        if (parameter.meleeManager.SurvivorIsInRange())
        {
            screamer.TransitionState(ScreamerStateType.Attack);
            return;
        }

        if (parameter.hasScreamed)
        {
            screamer.TransitionState(ScreamerStateType.Run);
            return;
        }

        screamer.TransitionState(ScreamerStateType.Walk);
    }

    public void OnExit()
    {

    }
}

public class SCWalkState : IState
{
    private Screamer screamer;

    private ScreamerParameter parameter;

    public SCWalkState(Screamer screamer)
    {
        this.screamer = screamer;

        this.parameter = screamer.parameter;
    }

    public void OnEnter()
    {
        parameter.animator.Play("Walk");
    }

    public void OnUpdate()
    {
        parameter.movementManager.Move();

        if (parameter.hasScreamed)
        {
            screamer.TransitionState(ScreamerStateType.Run);
            return;
        }
        if (parameter.meleeManager.SurvivorIsInRange())
        {
            screamer.TransitionState(ScreamerStateType.Attack);
            return;
        }

        if (parameter.aggroManager.currentTarget == null)
        {
            screamer.TransitionState(ScreamerStateType.Idle);
            return;
        }

    }

    public void OnExit()
    {

    }
}

public class SCRunState : IState
{
    private Screamer screamer;

    private ScreamerParameter parameter;

    public SCRunState(Screamer screamer)
    {
        this.screamer = screamer;

        this.parameter = screamer.parameter;
    }

    public void OnEnter()
    {
        parameter.animator.Play("Run");
        
        parameter.movementManager.speed = parameter.runSpeed;
    }

    public void OnUpdate()
    {
        parameter.movementManager.Move();

        if (parameter.meleeManager.SurvivorIsInRange())
        {
            screamer.TransitionState(ScreamerStateType.Attack);
            return;
        }

        if (parameter.aggroManager.currentTarget == null)
        {
            screamer.TransitionState(ScreamerStateType.Idle);
            return;
        }
    }

    public void OnExit()
    {

    }
}

public class SCJumpState : IState
{
    private Screamer screamer;

    private ScreamerParameter parameter;

    public SCJumpState(Screamer screamer)
    {
        this.screamer = screamer;

        this.parameter = screamer.parameter;
    }

    public void OnEnter()
    {

    }

    public void OnUpdate()
    {

    }

    public void OnExit()
    {

    }
}

public class SCAttackState : IState
{
    private Screamer screamer;

    private ScreamerParameter parameter;

    private float randomNum;

    private AnimatorStateInfo info;

    public SCAttackState(Screamer screamer)
    {
        this.screamer = screamer;

        this.parameter = screamer.parameter;
    }

    public void OnEnter()
    {
        randomNum = UnityEngine.Random.Range(1f, 100f);

        if (randomNum > 80f)
        {
            screamer.PerformComboAttack();
        }
        else if (randomNum < 26f)
        {
            parameter.animator.Play("Attack1");
        }
        else if (randomNum >= 26f && randomNum < 52f)
        {
            parameter.animator.Play("Attack2");
        }
        else if (randomNum > 52f && randomNum < 80f)
        {
            parameter.animator.Play("Attack3");
        }
    }

    public void OnUpdate()
    {
        info = parameter.animator.GetCurrentAnimatorStateInfo(0);
        if (parameter.isAttacking)
        {
            return;
        }
        if (!(info.IsName("Attack1") || info.IsName("Attack2") || info.IsName("Attack3")))
        {
            return;
        }

        if (info.normalizedTime > 1f)
        {
            screamer.TransitionState(ScreamerStateType.Idle);
            return;
        }
    }

    public void OnExit()
    {
        parameter.isAttacking = false;
    }
}

public class SCHurtState : IState
{
    private Screamer screamer;

    private ScreamerParameter parameter;

    public SCHurtState(Screamer screamer)
    {
        this.screamer = screamer;

        this.parameter = screamer.parameter;
    }

    public void OnEnter()
    {

    }

    public void OnUpdate()
    {

    }

    public void OnExit()
    {

    }
}

public class SCDieState : IState
{
    private Screamer screamer;

    private ScreamerParameter parameter;

    public SCDieState(Screamer screamer)
    {
        this.screamer = screamer;

        this.parameter = screamer.parameter;
    }

    public void OnEnter()
    {
        screamer.gameObject.tag = "Corpse";

        screamer.isDead = true;

        parameter.movementManager.DisableLinearVelocity();

        parameter.animator.Play("Die");
    }

    public void OnUpdate()
    {
        parameter.animator.Play("Die");
    }

    public void OnExit()
    {

    }
}

public class SCScreamState : IState
{
    private Screamer screamer;

    private ScreamerParameter parameter;

    private AnimatorStateInfo info;


    public SCScreamState(Screamer screamer)
    {
        this.screamer = screamer;

        this.parameter = screamer.parameter;
    }

    public void OnEnter()
    {
        parameter.isScreaming = true;

        parameter.animator.Play("Scream");

        parameter.hasScreamed = true;

        EventManager.RaiseOnScream(screamer.gameObject);

        Debug.Log("Screamer just screamed");
    }

    public void OnUpdate()
    {
        info = parameter.animator.GetCurrentAnimatorStateInfo(0);

        if (!info.IsName("Scream"))
        {
            return;
        }

        if (info.normalizedTime > 1f)
        {
            screamer.TransitionState(ScreamerStateType.Run);
        }
    }

    public void OnExit()
    {
        parameter.isScreaming = false;
    }
}


