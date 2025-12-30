
using UnityEngine;

public class TWalkState : IState
{
    private Tank tank;

    private TankParameter parameter;

    private bool shouldDoTusk = false;
    public TWalkState(Tank tank)
    {
        this.tank = tank;

        this.parameter = tank.parameter;
    }

    public void OnEnter()
    {
        float randomNum = UnityEngine.Random.Range(0, 100);
        parameter.animator.Play("Walk");

        if (randomNum > 70f)
        {
            shouldDoTusk = true;
        }
    }

    public void OnUpdate()
    {
        parameter.movementManager.Move();

        if (!parameter.meleeManager.SurvivorIsInRange())
        {
            return;
        }

        if (shouldDoTusk)
        {
            tank.TransitionState(TankStateType.TuskAttack);
            return;
        }
        else
        {
            tank.TransitionState(TankStateType.Attack);
            return;
        }
    }

    public void OnExit()
    {
        shouldDoTusk = false;
    }
}

public class THurtState : IState
{
    private Tank tank;

    private TankParameter parameter;
    public THurtState(Tank tank)
    {
        this.tank = tank;

        this.parameter = tank.parameter;
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

public class TAttackState : IState
{
    private Tank tank;

    private TankParameter parameter;

    private AnimatorStateInfo info;
    public TAttackState(Tank tank)
    {
        this.tank = tank;

        this.parameter = tank.parameter;
    }

    public void OnEnter()
    {
        parameter.movementManager.DisableLinearVelocity();

        parameter.animator.Play("Attack");
    }

    public void OnUpdate()
    {
        info = parameter.animator.GetCurrentAnimatorStateInfo(0);
        if (!info.IsName("Attack"))
        {
            return;
        }
        if (info.normalizedTime > 1f)
        {
            tank.TransitionState(TankStateType.Walk);
            return;
        }
    }

    public void OnExit()
    {

    }
}

public class TTuskAttackState : IState
{
    private Tank tank;

    private TankParameter parameter;

    private AnimatorStateInfo info;

    public TTuskAttackState(Tank tank)
    {
        this.tank = tank;

        this.parameter = tank.parameter;
    }

    public void OnEnter()
    {
        parameter.movementManager.DisableLinearVelocity();

        parameter.animator.Play("TuskAttack");
    }

    public void OnUpdate()
    {
        info = parameter.animator.GetCurrentAnimatorStateInfo(0);
        if (!info.IsName("TuskAttack"))
        {
            return;
        }
        if (info.normalizedTime > 1f)
        {
            tank.TransitionState(TankStateType.Walk);
            return;
        }
    }

    public void OnExit()
    {

    }
}

public class TDieState : IState
{
    private Tank tank;

    private TankParameter parameter;
    public TDieState(Tank tank)
    {
        this.tank = tank;

        this.parameter = tank.parameter;
    }

    public void OnEnter()
    {
        tank.gameObject.tag = "Corpse";

        tank.isDead = true;

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


