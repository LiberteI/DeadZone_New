using UnityEngine;

public class CIdleState : IState
{
    private Clutcher clutcher;

    private ClutcherParameter parameter;

    public CIdleState(Clutcher clutcher)
    {
        this.clutcher = clutcher;

        this.parameter = clutcher.parameter;
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
            clutcher.TransitionState(ClutcherStateType.Clutch);
            return;
        }
        clutcher.TransitionState(ClutcherStateType.Walk);
        return;
    }

    public void OnExit()
    {

    }
}

public class CClutchState : IState
{
    private Clutcher clutcher;

    private ClutcherParameter parameter;

    private AnimatorStateInfo info;

    public CClutchState(Clutcher clutcher)
    {
        this.clutcher = clutcher;

        this.parameter = clutcher.parameter;
    }

    public void OnEnter()
    {
        parameter.movementManager.DisableLinearVelocity();

        parameter.animator.Play("Clutch");
    }

    public void OnUpdate()
    {
        info = parameter.animator.GetCurrentAnimatorStateInfo(0);
        if (!info.IsName("Clutch"))
        {
            return;
        }
        if (info.normalizedTime > 1f)
        {
            clutcher.TransitionState(ClutcherStateType.Idle);
            return;
        }
    }

    public void OnExit()
    {

    }
}

public class CHurtState : IState
{
    private Clutcher clutcher;

    private ClutcherParameter parameter;

    public CHurtState(Clutcher clutcher)
    {
        this.clutcher = clutcher;

        this.parameter = clutcher.parameter;
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

public class CDieState : IState
{
    private Clutcher clutcher;

    private ClutcherParameter parameter;

    public CDieState(Clutcher clutcher)
    {
        this.clutcher = clutcher;

        this.parameter = clutcher.parameter;
    }

    public void OnEnter()
    {
        clutcher.gameObject.tag = "Corpse";

        clutcher.isDead = true;

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

public class CWalkState : IState
{
    private Clutcher clutcher;

    private ClutcherParameter parameter;

    public CWalkState(Clutcher clutcher)
    {
        this.clutcher = clutcher;

        this.parameter = clutcher.parameter;
    }

    public void OnEnter()
    {
        parameter.animator.Play("Walk");
    }

    public void OnUpdate()
    {
        parameter.movementManager.Move();

        if (parameter.meleeManager.SurvivorIsInRange())
        {
            clutcher.TransitionState(ClutcherStateType.Clutch);
            return;
        }

        if (parameter.aggroManager.currentTarget == null)
        {
            clutcher.TransitionState(ClutcherStateType.Idle);
            return;
        }
    }

    public void OnExit()
    {
        
    }
}
