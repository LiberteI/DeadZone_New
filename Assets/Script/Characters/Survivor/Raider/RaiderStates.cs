using UnityEngine;

public class RaiderIdleState : SurvivorIState
{
    private Raider survivor;

    private RaiderParameter parameter;

    public RaiderIdleState(Raider survivor)
    {
        this.survivor = survivor;

        this.parameter = survivor.parameter;
    }
    public void OnEnter()
    {
        parameter.movementManager.DisableLinearVelocity();

        parameter.animator.Play("Idle");

        parameter.movementManager.isRunning = false;
    }

    public void OnUpdate()
    {
        if (survivor.isPlayedByPlayer)
        {
            return;
        }
        if (parameter.aiManager.shouldFollow)
        {
            survivor.TransitionState(RaiderStateType.Walk);
            return;
        }
    }

    public void HandleInput()
    {
        if (Input.GetKeyDown("r"))
        {
            survivor.TransitionState(RaiderStateType.Reload);
            return;
        }
        if (Input.GetKeyDown("f"))
        {
            survivor.TransitionState(RaiderStateType.Melee);
            return;
        }
        if (Input.GetKey("j"))
        {
            if (!parameter.shootingManager.CanShoot())
            {
                return;
            }
            survivor.TransitionState(RaiderStateType.Shoot);
            return;
        }
        // movement
        if (Input.GetKey("a") || Input.GetKey("d"))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                survivor.TransitionState(RaiderStateType.Run);
                return;
            }
            survivor.TransitionState(RaiderStateType.Walk);
            return;
        }

        if (Input.GetKeyDown("k"))
        {
            if (parameter.movementManager.CanJump())
            {
                survivor.TransitionState(RaiderStateType.Jump);
                return;
            }

        }

    }
    public void OnExit()
    {

    }
}

public class RaiderShootState : SurvivorIState
{
    private Raider survivor;

    private RaiderParameter parameter;

    private AnimatorStateInfo info;

    public RaiderShootState(Raider survivor)
    {
        this.survivor = survivor;

        this.parameter = survivor.parameter;
    }
    public void OnEnter()
    {
        parameter.shootingManager.SetShootTimer();

        parameter.isShooting = true;

        parameter.animator.Play("Shoot");

        parameter.shootingManager.FireABullet(parameter.standMuzzle);
    }

    public void OnUpdate()
    {
        info = parameter.animator.GetCurrentAnimatorStateInfo(0);
        if (!info.IsName("Shoot"))
        {
            parameter.animator.Play("Shoot");
            return;
        }
        if (info.normalizedTime < 1f)
        {
            return;
        }
        survivor.TransitionState(RaiderStateType.Idle);
        return;
    }

    public void HandleInput()
    {

    }
    public void OnExit()
    {
        parameter.isShooting = false;
    }
}

public class RaiderRunState : SurvivorIState
{
    private Raider survivor;

    private RaiderParameter parameter;

    public RaiderRunState(Raider survivor)
    {
        this.survivor = survivor;

        this.parameter = survivor.parameter;
    }
    public void OnEnter()
    {
        parameter.movementManager.isRunning = true;
        
        parameter.animator.Play("Run");
    }

    public void OnUpdate()
    {

    }

    public void HandleInput()
    {
        if(Input.GetKeyDown("r")){
            survivor.TransitionState(RaiderStateType.Reload);
            return;
        }
        if(Input.GetKeyDown("f")){
            survivor.TransitionState(RaiderStateType.Melee);
            return;
        }
        if(Input.GetKey("j")){
            if (!parameter.shootingManager.CanShoot())
            {
                return;
            }
            survivor.TransitionState(RaiderStateType.Shoot);
            return;

        }
        if(!Input.GetKey(KeyCode.LeftShift)){
            survivor.TransitionState(RaiderStateType.Walk);
            return;
        }
        if(!(Input.GetKey("a") || Input.GetKey("d"))){
            survivor.TransitionState(RaiderStateType.Idle);
            return;
        }
        if(Input.GetKeyDown("k")){
            if(parameter.movementManager.CanJump()){
                survivor.TransitionState(RaiderStateType.Jump);
                return;
            }
            
        }
    }
    public void OnExit()
    {

    }
}

public class RaiderJumpState : SurvivorIState
{
    private Raider survivor;

    private RaiderParameter parameter;

    public RaiderJumpState(Raider survivor)
    {
        this.survivor = survivor;

        this.parameter = survivor.parameter;
    }
    public void OnEnter()
    {
        parameter.animator.Play("Jump");

        parameter.movementManager.Jump();
    }

    public void OnUpdate()
    {
        if (parameter.movementManager.isInMidAir)
        {
            return;
        }
        survivor.TransitionState(RaiderStateType.Idle);
    }

    public void HandleInput()
    {

    }
    public void OnExit()
    {

    }
}

public class RaiderHurtState : SurvivorIState
{
    private Raider survivor;

    private RaiderParameter parameter;

    public RaiderHurtState(Raider survivor)
    {
        this.survivor = survivor;

        this.parameter = survivor.parameter;
    }
    public void OnEnter()
    {
        
    }

    public void OnUpdate()
    {

    }

    public void HandleInput()
    {

    }
    public void OnExit()
    {
        
    }
}

public class RaiderMeleeState : SurvivorIState
{
    private Raider survivor;

    private RaiderParameter parameter;

    public RaiderMeleeState(Raider survivor)
    {
        this.survivor = survivor;

        this.parameter = survivor.parameter;
    }
    public void OnEnter()
    {
        parameter.isDoingMelee = true;

        parameter.meleeManager.Attack();
    }

    public void OnUpdate()
    {
        if (parameter.meleeManager.isInCoroutine)
        {
            return;
        }

        survivor.TransitionState(RaiderStateType.Idle);
    }

    public void HandleInput()
    {

    }
    public void OnExit()
    {
        parameter.isDoingMelee = false;
    }
}

public class RaiderDieState : SurvivorIState
{
    private Raider survivor;

    private RaiderParameter parameter;

    public RaiderDieState(Raider survivor)
    {
        this.survivor = survivor;

        this.parameter = survivor.parameter;
    }
    public void OnEnter()
    {

    }

    public void OnUpdate()
    {

    }

    public void HandleInput()
    {

    }
    public void OnExit()
    {

    }
}

public class RaiderWalkState : SurvivorIState
{
    private Raider survivor;

    private RaiderParameter parameter;

    public RaiderWalkState(Raider survivor)
    {
        this.survivor = survivor;

        this.parameter = survivor.parameter;
    }
    public void OnEnter()
    {
        parameter.movementManager.isRunning = false;

        parameter.animator.Play("Walk");
    }

    public void OnUpdate()
    {
        if (survivor.isPlayedByPlayer)
        {
            parameter.movementManager.Walk();
            return;
        }
        if (!parameter.aiManager.shouldFollow)
        {
            survivor.TransitionState(RaiderStateType.Idle);
            return;
        }

        parameter.movementManager.Walk(Mathf.Sign(parameter.aiManager.distance));
    }

    public void HandleInput()
    {
        if (Input.GetKeyDown("r"))
        {
            survivor.TransitionState(RaiderStateType.Reload);
            return;
        }
        if (Input.GetKeyDown("f"))
        {
            survivor.TransitionState(RaiderStateType.Melee);
            return;
        }
        if (Input.GetKey("j"))
        {
            if (!parameter.shootingManager.CanShoot())
            {
                return;
            }
            survivor.TransitionState(RaiderStateType.Shoot);
            return;
        }
        if (!(Input.GetKey("a") || Input.GetKey("d")))
        {
            survivor.TransitionState(RaiderStateType.Idle);
            return;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            survivor.TransitionState(RaiderStateType.Run);
            return;
        }
        if (Input.GetKeyDown("k"))
        {
            if (parameter.movementManager.CanJump())
            {
                survivor.TransitionState(RaiderStateType.Jump);
                return;
            }

        }
    }
    public void OnExit()
    {

    }
}

public class RaiderReloadState : SurvivorIState
{
    private Raider survivor;

    private RaiderParameter parameter;

    private AnimatorStateInfo info;

    public RaiderReloadState(Raider survivor)
    {
        this.survivor = survivor;

        this.parameter = survivor.parameter;
    }
    public void OnEnter()
    {
        parameter.isReloading = true;
        parameter.animator.Play("Reload");
        info = parameter.animator.GetCurrentAnimatorStateInfo(0);
    }

    public void OnUpdate()
    {
        info = parameter.animator.GetCurrentAnimatorStateInfo(0);

        if(!info.IsName("Reload")){
            return;
        }
        if(info.normalizedTime > 1f){
            survivor.TransitionState(RaiderStateType.Idle);
            return;
        }
    }

    public void HandleInput()
    {
        
    }
    public void OnExit()
    {
        parameter.isReloading = false;
    }
}


