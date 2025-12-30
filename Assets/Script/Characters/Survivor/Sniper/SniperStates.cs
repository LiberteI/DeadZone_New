using UnityEngine;

public class SniperIdleState : SurvivorIState
{
    private Sniper survivor;

    private SniperParameter parameter;

    public SniperIdleState(Sniper survivor)
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
            survivor.TransitionState(SniperStateType.Walk);
            return;
        }
    }
    public void OnExit()
    {

    }

    public void HandleInput()
    {
        if (Input.GetKeyDown("r"))
        {
            survivor.TransitionState(SniperStateType.Reload);
            return;
        }
        if (Input.GetKeyDown("f"))
        {
            survivor.TransitionState(SniperStateType.Melee);
            return;
        }
        if (Input.GetKey("j"))
        {
            if (!parameter.shootingManager.CanShoot())
            {
                return;
            }
            if (Input.GetKey("s"))
            {
                survivor.TransitionState(SniperStateType.CrouchShoot);
                return;
            }
            survivor.TransitionState(SniperStateType.StandShoot);
            return;
        }
        // movement
        if (Input.GetKey("a") || Input.GetKey("d"))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                survivor.TransitionState(SniperStateType.Run);
                return;
            }
            survivor.TransitionState(SniperStateType.Walk);
            return;
        }

        if (Input.GetKeyDown("k"))
        {
            if (parameter.movementManager.CanJump())
            {
                survivor.TransitionState(SniperStateType.Jump);
                return;
            }

        }

        if (Input.GetKey("s"))
        {
            survivor.TransitionState(SniperStateType.Crouch);
        }
    }
}
public class SniperStandShootState : SurvivorIState
{
    private Sniper sniper;

    private SniperParameter parameter;

    private AnimatorStateInfo info;

    public SniperStandShootState(Sniper sniper)
    {
        this.sniper = sniper;

        this.parameter = sniper.parameter;
    }

    public void OnEnter()
    {
        parameter.shootingManager.SetShootTimer();

        parameter.isShooting = true;

        parameter.animator.Play("StandShoot");

        parameter.shootingManager.FireABullet(parameter.standMuzzle);
    }

    public void OnUpdate()
    {
        info = parameter.animator.GetCurrentAnimatorStateInfo(0);
        if (!info.IsName("StandShoot"))
        {
            parameter.animator.Play("StandShoot");
            return;
        }
        if (info.normalizedTime < 1f)
        {
            return;
        }
        sniper.TransitionState(SniperStateType.Idle);
        return;
    }

    public void HandleInput()
    {
        if(Input.GetKey("s")){
            if (!parameter.shootingManager.CanShoot())
            {
                sniper.TransitionState(SniperStateType.Crouch);
                return;
            }
            sniper.TransitionState(SniperStateType.CrouchShoot);
            return;
        }

    }

    public void OnExit()
    {
        parameter.isShooting = false;
    }
}
public class SniperCrouchShootState : SurvivorIState
{
    private Sniper sniper;

    private SniperParameter parameter;

    private AnimatorStateInfo info;

    public SniperCrouchShootState(Sniper sniper)
    {
        this.sniper = sniper;

        this.parameter = sniper.parameter;
    }

    public void OnEnter()
    {
        parameter.shootingManager.SetShootTimer();

        parameter.isShooting = true;

        parameter.animator.Play("CrouchShoot");

        parameter.shootingManager.FireABullet(parameter.crouchMuzzle);
    }

    public void OnUpdate()
    {
        info = parameter.animator.GetCurrentAnimatorStateInfo(0);
        if (!info.IsName("CrouchShoot"))
        {
            parameter.animator.Play("CrouchShoot");
            return;
        }
        if (info.normalizedTime < 1f)
        {
            return;
        }
        sniper.TransitionState(SniperStateType.Crouch);
        return;
    }

    public void HandleInput()
    {
        if(!Input.GetKey("s")){
            sniper.TransitionState(SniperStateType.Idle);
            return;
        }
    }

    public void OnExit()
    {
        parameter.isShooting = false;
    }
}
public class SniperWalkState : SurvivorIState
{
    private Sniper survivor;

    private SniperParameter parameter;

    public SniperWalkState(Sniper survivor)
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
            survivor.TransitionState(SniperStateType.Idle);
            return;
        }

        parameter.movementManager.Walk(Mathf.Sign(parameter.aiManager.distance));
    }
    public void OnExit()
    {

    }

    public void HandleInput()
    {
        if (Input.GetKeyDown("r"))
        {
            survivor.TransitionState(SniperStateType.Reload);
            return;
        }
        if (Input.GetKeyDown("f"))
        {
            survivor.TransitionState(SniperStateType.Melee);
            return;
        }
        if (Input.GetKey("j"))
        {
            if (Input.GetKey("s"))
            {
                survivor.TransitionState(SniperStateType.CrouchShoot);
                return;
            }
            survivor.TransitionState(SniperStateType.StandShoot);
            return;
        }
        if (!(Input.GetKey("a") || Input.GetKey("d")))
        {
            survivor.TransitionState(SniperStateType.Idle);
            return;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            survivor.TransitionState(SniperStateType.Run);
            return;
        }
        if (Input.GetKeyDown("k"))
        {
            if (parameter.movementManager.CanJump())
            {
                survivor.TransitionState(SniperStateType.Jump);
                return;
            }

        }
        if (Input.GetKey("s"))
        {
            survivor.TransitionState(SniperStateType.Crouch);
        }
    }
}

public class SniperRunState : SurvivorIState
{
    private Sniper sniper;

    private SniperParameter parameter;

    public SniperRunState(Sniper sniper)
    {
        this.sniper = sniper;

        this.parameter = sniper.parameter;
    }

    public void OnEnter()
    {
        parameter.movementManager.isRunning = true;
        
        parameter.animator.Play("Run");
    }

    public void OnUpdate()
    {
        if (sniper.isPlayedByPlayer)
        {
            parameter.movementManager.Run();
        }
    }

    public void HandleInput()
    {
        if(Input.GetKeyDown("r")){
            sniper.TransitionState(SniperStateType.Reload);
            return;
        }
        if(Input.GetKeyDown("f")){
            sniper.TransitionState(SniperStateType.Melee);
            return;
        }
        if(Input.GetKey("j")){
            if (!parameter.shootingManager.CanShoot())
            {
                return;
            }
            if (Input.GetKey("s"))
            {
                sniper.TransitionState(SniperStateType.CrouchShoot);
                return;
            }
            sniper.TransitionState(SniperStateType.StandShoot);
            return;

        }
        if(!Input.GetKey(KeyCode.LeftShift)){
            sniper.TransitionState(SniperStateType.Walk);
            return;
        }
        if(!(Input.GetKey("a") || Input.GetKey("d"))){
            sniper.TransitionState(SniperStateType.Idle);
            return;
        }
        if(Input.GetKeyDown("k")){
            if(parameter.movementManager.CanJump()){
                sniper.TransitionState(SniperStateType.Jump);
                return;
            }
            
        }
        if(Input.GetKey("s")){
            sniper.TransitionState(SniperStateType.Crouch);
        }
    }

    public void OnExit()
    {

    }
}

public class SniperCrouchState : SurvivorIState
{
    private Sniper sniper;

    private SniperParameter parameter;

    public SniperCrouchState(Sniper sniper)
    {
        this.sniper = sniper;

        this.parameter = sniper.parameter;
    }

    public void OnEnter()
    {
        parameter.isCrouching = true;

        parameter.animator.Play("Crouch");
    }

    public void OnUpdate()
    {

    }

    public void HandleInput()
    {
        if(!Input.GetKey("s")){
            sniper.TransitionState(SniperStateType.Idle);
            return;
        }
        if(Input.GetKey("j")){
            if (!parameter.shootingManager.CanShoot())
            {
                return;
            }
            sniper.TransitionState(SniperStateType.CrouchShoot);
            return;
        }
    }

    public void OnExit()
    {
        parameter.isCrouching = false;
    }
}

public class SniperThrowState : SurvivorIState
{
    private Sniper sniper;

    private SniperParameter parameter;

    public SniperThrowState(Sniper sniper)
    {
        this.sniper = sniper;

        this.parameter = sniper.parameter;
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

public class SniperReloadState : SurvivorIState
{
    private Sniper sniper;

    private SniperParameter parameter;

    private AnimatorStateInfo info;

    public SniperReloadState(Sniper sniper)
    {
        this.sniper = sniper;

        this.parameter = sniper.parameter;
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
            sniper.TransitionState(SniperStateType.Idle);
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

public class SniperJumpState : SurvivorIState
{
    private Sniper sniper;

    private SniperParameter parameter;

    public SniperJumpState(Sniper sniper)
    {
        this.sniper = sniper;

        this.parameter = sniper.parameter;
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
        sniper.TransitionState(SniperStateType.Idle);
    }

    public void HandleInput()
    {

    }

    public void OnExit()
    {

    }
}

public class SniperHurtState : SurvivorIState
{
    private Sniper sniper;

    private SniperParameter parameter;

    public SniperHurtState(Sniper sniper)
    {
        this.sniper = sniper;

        this.parameter = sniper.parameter;
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

public class SniperDieState : SurvivorIState
{
    private Sniper sniper;

    private SniperParameter parameter;

    public SniperDieState(Sniper sniper)
    {
        this.sniper = sniper;

        this.parameter = sniper.parameter;
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



public class SniperMeleeState : SurvivorIState
{
    private Sniper sniper;

    private SniperParameter parameter;

    private AnimatorStateInfo info;

    public SniperMeleeState(Sniper sniper)
    {
        this.sniper = sniper;

        this.parameter = sniper.parameter;
    }

    public void OnEnter()
    {
        parameter.isDoingMelee = true;
        parameter.animator.Play("Melee");
        info = parameter.animator.GetCurrentAnimatorStateInfo(0);
    }

    public void OnUpdate()
    {
        info = parameter.animator.GetCurrentAnimatorStateInfo(0);
        if(!info.IsName("Melee")){
            return;
        }
        if(info.normalizedTime > 1f){
            sniper.TransitionState(SniperStateType.Idle);
            return;
        }
    }

    public void HandleInput()
    {
        
    }

    public void OnExit()
    {
        parameter.isDoingMelee = false;
    }
}

