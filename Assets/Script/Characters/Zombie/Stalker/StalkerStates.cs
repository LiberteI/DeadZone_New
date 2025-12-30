
using UnityEngine;

public class SIdleState : IState
{
    private Stalker stalker;

    private StalkerParameter parameter;

    private float random;
    public SIdleState(Stalker stalker)
    {
        this.stalker = stalker;

        this.parameter = stalker.parameter;
    }

    public void OnEnter()
    {
        random = UnityEngine.Random.Range(0, 100);

        parameter.movementManager.DisableLinearVelocity();
        // half chance to play each idle phase
        if (random > 50f)
        {
            foreach (var curClip in parameter.animator.runtimeAnimatorController.animationClips)
            {
                if (curClip.name == "Idle2")
                {
                    parameter.animator.Play("Idle2");
                    return;
                }
            }

            parameter.animator.Play("Idle");
            return;
        }
        
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
            stalker.TransitionState(StalkerStateType.Attack);
            return;
        }

        if (parameter.hasBeenProvoked)
        {
            stalker.TransitionState(StalkerStateType.Run);
            return;
        }

        stalker.TransitionState(StalkerStateType.Walk);
    }

    public void OnExit()
    {

    }
}

public class SWalkState : IState
{
    private Stalker stalker;

    private StalkerParameter parameter;

    public SWalkState(Stalker stalker)
    {
        this.stalker = stalker;

        this.parameter = stalker.parameter;
    }

    public void OnEnter()
    {
        parameter.animator.Play("Walk");
    }

    public void OnUpdate()
    {
        parameter.movementManager.Move();

        if (parameter.hasBeenProvoked)
        {
            stalker.TransitionState(StalkerStateType.Run);
            return;
        }

        if (parameter.meleeManager.SurvivorIsInRange())
        {
            stalker.TransitionState(StalkerStateType.Attack);
            return;
        }

        if (parameter.aggroManager.currentTarget == null)
        {
            stalker.TransitionState(StalkerStateType.Idle);
            return;
        }
    }

    public void OnExit()
    {

    }
}

public class SRunState : IState
{
    private Stalker stalker;

    private StalkerParameter parameter;

    public SRunState(Stalker stalker)
    {
        this.stalker = stalker;

        this.parameter = stalker.parameter;
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
            stalker.TransitionState(StalkerStateType.Attack);
            return;
        }

        if (parameter.aggroManager.currentTarget == null)
        {
            stalker.TransitionState(StalkerStateType.Idle);
            return;
        }
    }

    public void OnExit()
    {

    }
}

public class SAttackState : IState
{
    private Stalker stalker;

    private StalkerParameter parameter;

    private float randomNum;

    private AnimatorStateInfo info;

    public SAttackState(Stalker stalker)
    {
        this.stalker = stalker;

        this.parameter = stalker.parameter;
    }

    public void OnEnter()
    {
        randomNum = UnityEngine.Random.Range(1f, 100f);

        if (randomNum > 80f)
        {
            stalker.PerformComboAttack();
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
            stalker.TransitionState(StalkerStateType.Idle);
            return;
        }
    }

    public void OnExit()
    {
        parameter.isAttacking = false;
    }
}

public class SHurtState : IState
{
    private Stalker stalker;

    private StalkerParameter parameter;

    public SHurtState(Stalker stalker)
    {
        this.stalker = stalker;

        this.parameter = stalker.parameter;
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

public class SDieState : IState
{
    private Stalker stalker;

    private StalkerParameter parameter;

    public SDieState(Stalker stalker)
    {
        this.stalker = stalker;

        this.parameter = stalker.parameter;
    }

    public void OnEnter()
    {
        stalker.gameObject.tag = "Corpse";

        stalker.isDead = true;

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

public class SJumpState : IState
{
    private Stalker stalker;

    private StalkerParameter parameter;

    public SJumpState(Stalker stalker)
    {
        this.stalker = stalker;

        this.parameter = stalker.parameter;
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
