using UnityEngine;

public class PIdleState : IState
{
    private Poisoner poisoner;

    private PoisonerParameter parameter;

    private float random;

    private float decideTimer;

    public PIdleState(Poisoner poisoner)
    {
        this.poisoner = poisoner;

        this.parameter = poisoner.parameter;
    }
    public void OnEnter()
    {
        decideTimer = 2f;

        random = UnityEngine.Random.Range(0, 100);

        if (random > 50f)
        {
            parameter.animator.Play("Idle2");
        }
        else
        {
            parameter.animator.Play("Idle");
        }
    }

    public void OnUpdate()
    {
        parameter.movementManager.FlipToTarget();
        
        if (decideTimer > 0)
        {
            decideTimer -= Time.deltaTime;
            return;
        }
        if (parameter.aggroManager.currentTarget == null)
        {
            return;
        }
        if (parameter.meleeManager.SurvivorIsInRange())
        {
            poisoner.TransitionState(PoisonerStateType.Attack);
            return;
        }
        if (((PoisonerMovement)parameter.movementManager).shouldProject)
        {
            poisoner.TransitionState(PoisonerStateType.Project);
            return;
        }
        poisoner.TransitionState(PoisonerStateType.Walk);
    }

    public void OnExit()
    {
        decideTimer = 2f;
    }
}

public class PWalkState : IState
{
    private Poisoner poisoner;

    private PoisonerParameter parameter;

    public PWalkState(Poisoner poisoner)
    {
        this.poisoner = poisoner;

        this.parameter = poisoner.parameter;
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
            poisoner.TransitionState(PoisonerStateType.Attack);
            return;
        }

        if (((PoisonerMovement)parameter.movementManager).shouldProject)
        {
            poisoner.TransitionState(PoisonerStateType.Project);
            return;
        }
    }

    public void OnExit()
    {

    }
}

public class PAttackState : IState
{
    private Poisoner poisoner;

    private PoisonerParameter parameter;

    private AnimatorStateInfo info;


    public PAttackState(Poisoner poisoner)
    {
        this.poisoner = poisoner;

        this.parameter = poisoner.parameter;
    }
    public void OnEnter()
    {
        parameter.animator.Play("Attack1");
    }

    public void OnUpdate()
    {
        info = parameter.animator.GetCurrentAnimatorStateInfo(0);
        parameter.movementManager.FlipToTarget();
        if (info.normalizedTime < 1f)
        {
            return;
        }

        poisoner.TransitionState(PoisonerStateType.Idle);
    }

    public void OnExit()
    {

    }
}

public class PProjectState : IState
{
    private Poisoner poisoner;

    private PoisonerParameter parameter;

    public PProjectState(Poisoner poisoner)
    {
        this.poisoner = poisoner;

        this.parameter = poisoner.parameter;
    }
    public void OnEnter()
    {
        // do spit
        poisoner.ProjectPoisonousSpore();
    }

    public void OnUpdate()
    {
        if (parameter.isProjecting)
        {
            return;
        }
        poisoner.TransitionState(PoisonerStateType.Idle);
        
    }

    public void OnExit()
    {

    }
}

public class PHurtState : IState
{
    private Poisoner poisoner;

    private PoisonerParameter parameter;

    public PHurtState(Poisoner poisoner)
    {
        this.poisoner = poisoner;

        this.parameter = poisoner.parameter;
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

public class PDieState : IState
{
    private Poisoner poisoner;

    private PoisonerParameter parameter;

    public PDieState(Poisoner poisoner)
    {
        this.poisoner = poisoner;

        this.parameter = poisoner.parameter;
    }
    public void OnEnter()
    {
        poisoner.gameObject.tag = "Corpse";

        poisoner.isDead = true;

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

public class PJumpState : IState
{
    private Poisoner poisoner;

    private PoisonerParameter parameter;

    public PJumpState(Poisoner poisoner)
    {
        this.poisoner = poisoner;

        this.parameter = poisoner.parameter;
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

