using UnityEngine;

public class BIdleState : IState
{
    private Boomer boomer;

    private BoomerParameter parameter;

    public BIdleState(Boomer boomer)
    {
        this.boomer = boomer;

        this.parameter = boomer.parameter;
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
            boomer.TransitionState(BoomerStateType.Attack);
            return;
        }
        boomer.TransitionState(BoomerStateType.Walk);
    }

    public void OnExit()
    {

    }
}

public class BWalkState : IState
{
    private Boomer boomer;

    private BoomerParameter parameter;

    public BWalkState(Boomer boomer)
    {
        this.boomer = boomer;

        this.parameter = boomer.parameter;
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
            boomer.TransitionState(BoomerStateType.Attack);
            return;
        }

        if (parameter.aggroManager.currentTarget == null)
        {
            boomer.TransitionState(BoomerStateType.Idle);
            return;
        }
    }

    public void OnExit()
    {

    }
}

public class BHurtState : IState
{
    private Boomer boomer;

    private BoomerParameter parameter;

    public BHurtState(Boomer boomer)
    {
        this.boomer = boomer;

        this.parameter = boomer.parameter;
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


public class BAttackState : IState
{
    private Boomer boomer;

    private BoomerParameter parameter;

    private AnimatorStateInfo info;

    public BAttackState(Boomer boomer)
    {
        this.boomer = boomer;

        this.parameter = boomer.parameter;
    }

    public void OnEnter()
    {
        parameter.movementManager.DisableLinearVelocity();

        parameter.animator.Play("Attack");
    }

    public void OnUpdate()
    {
        info = parameter.animator.GetCurrentAnimatorStateInfo(0);
        if(!info.IsName("Attack")){
            return;
        }
        if(info.normalizedTime > 1f){
            boomer.TransitionState(BoomerStateType.Idle);
            return;
        }
    }

    public void OnExit()
    {
        
    }
}


public class BExplodeState : IState
{
    private Boomer boomer;

    private BoomerParameter parameter;

    public BExplodeState(Boomer boomer)
    {
        this.boomer = boomer;

        this.parameter = boomer.parameter;
    }

    public void OnEnter()
    {
        boomer.gameObject.tag = "Corpse";

        boomer.isDead = true;

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

