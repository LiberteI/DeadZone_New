using UnityEditorInternal;
using UnityEngine;

public class JIdleState : IState
{
    private Jockey jockey;

    private JockeyParameter parameter;

    public JIdleState(Jockey jockey)
    {
        this.jockey = jockey;

        this.parameter = jockey.parameter;
    }

    public void OnEnter()
    {
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
            jockey.TransitionState(JockeyStateType.Attack);
        }

        jockey.TransitionState(JockeyStateType.Crawl);
    }
    public void OnExit()
    {

    }
}

public class JCrawlState : IState
{
    private Jockey jockey;

    private JockeyParameter parameter;

    public JCrawlState(Jockey jockey)
    {
        this.jockey = jockey;

        this.parameter = jockey.parameter;
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
            jockey.TransitionState(JockeyStateType.Attack);
        }
        
    }
    public void OnExit()
    {
        
    }
}

public class JJumpState : IState
{
    private Jockey jockey;

    private JockeyParameter parameter;

    public JJumpState(Jockey jockey)
    {
        this.jockey = jockey;

        this.parameter = jockey.parameter;
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


public class JHuntState : IState
{
    private Jockey jockey;

    private JockeyParameter parameter;

    public JHuntState(Jockey jockey)
    {
        this.jockey = jockey;

        this.parameter = jockey.parameter;
    }

    public void OnEnter()
    {
        jockey.TryHunt();
    }
    public void OnUpdate()
    {
        if (jockey.isHunting)
        {
            return;
        }
        parameter.movementManager.DisableLinearVelocity();
        if (!parameter.attackShouldLoop)
        {
            parameter.controlBox.SetActive(false);

            parameter.detectionBox.SetActive(false);

            jockey.TransitionState(JockeyStateType.Run);
            return;
        }
        
        // neutral state for performing hunt
    }
    public void OnExit()
    {
        parameter.controlBox.SetActive(false);
    }
}

public class JAttackState : IState
{
    private Jockey jockey;

    private JockeyParameter parameter;

    private float randomNum;

    private AnimatorStateInfo info;

    public JAttackState(Jockey jockey)
    {
        this.jockey = jockey;

        this.parameter = jockey.parameter;
    }

    public void OnEnter()
    {
        parameter.movementManager.DisableLinearVelocity();

        randomNum = UnityEngine.Random.Range(1f, 100f);

        if (randomNum > 80f)
        {
            jockey.PerformComboAttack();
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
        if (info.normalizedTime < 1f)
        {
            info = parameter.animator.GetCurrentAnimatorStateInfo(0);
            return;
        }
        if (jockey.parameter.isAttacking)
        {
            return;
        }
        if (parameter.attackShouldLoop)
        {
            jockey.TransitionState(JockeyStateType.Attack);

            return;
        }
        if (parameter.meleeManager.SurvivorIsInRange())
        {
            jockey.TransitionState(JockeyStateType.Attack);
            return;
        }

        jockey.TransitionState(JockeyStateType.Run);
    }
    public void OnExit()
    {

    }
}

public class JHurtState : IState
{
    private Jockey jockey;

    private JockeyParameter parameter;

    public JHurtState(Jockey jockey)
    {
        this.jockey = jockey;

        this.parameter = jockey.parameter;
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

public class JDieState : IState
{
    private Jockey jockey;

    private JockeyParameter parameter;

    public JDieState(Jockey jockey)
    {
        this.jockey = jockey;

        this.parameter = jockey.parameter;
    }

    public void OnEnter()
    {
        jockey.gameObject.tag = "Corpse";

        jockey.isDead = true;

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

public class JRunState : IState
{
    private Jockey jockey;

    private JockeyParameter parameter;

    public JRunState(Jockey jockey)
    {
        this.jockey = jockey;

        this.parameter = jockey.parameter;
    }

    public void OnEnter()
    {
        parameter.animator.Play("Run");

        parameter.movementManager.speed = 10f;
    }
    public void OnUpdate()
    {
        parameter.movementManager.Move();
        if (parameter.meleeManager.SurvivorIsInRange())
        {
            jockey.TransitionState(JockeyStateType.Attack);
            return;
        }
    }
    public void OnExit()
    {

    }
}
