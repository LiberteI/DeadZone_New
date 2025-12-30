using UnityEngine;

public class SWATIdleState : SurvivorIState
{
    private SWAT swat;
    
    private SWATParameter parameter;

    public SWATIdleState(SWAT swat){
        this.swat = swat;

        this.parameter = swat.parameter;
    }

    public void OnEnter(){
        parameter.movementManager.DisableLinearVelocity();

        parameter.movementManager.isRunning = false;

        parameter.animator.Play("Idle");
    }

    public void OnUpdate()
    {
        if (swat.isPlayedByPlayer)
        {
            return;
        }
        if (parameter.aiManager.shouldFollow)
        {
            swat.TransitionState(SWATStateType.Walk);
            return;
        }

    }

    public void HandleInput(){
        if(Input.GetKeyDown("r")){
            swat.TransitionState(SWATStateType.Reload);
            return;
        }
        if(Input.GetKeyDown("f")){
            swat.TransitionState(SWATStateType.Melee);
            return;
        }
        if(Input.GetKey("j")){
            if(Input.GetKey("s")){
                swat.TransitionState(SWATStateType.CrouchShoot);
                return;
            }
            swat.TransitionState(SWATStateType.StandShoot);
            return;
        }
        // movement
        if(Input.GetKey("a") || Input.GetKey("d")){
            if(Input.GetKey(KeyCode.LeftShift)){
                swat.TransitionState(SWATStateType.Run);
                return;
            }
            swat.TransitionState(SWATStateType.Walk);
            return;
        }

        if(Input.GetKeyDown("k")){
            if(parameter.movementManager.CanJump()){
                swat.TransitionState(SWATStateType.Jump);
                return;
            }
            
        }

        if(Input.GetKey("s")){
            swat.TransitionState(SWATStateType.Crouch);
        }
        
    }

    public void OnExit(){

    }
}

public class SWATWalkState : SurvivorIState
{
    private SWAT swat;
    
    private SWATParameter parameter;

    public SWATWalkState(SWAT swat){
        this.swat = swat;

        this.parameter = swat.parameter;
    }

    public void OnEnter(){
        parameter.movementManager.isRunning = false;
        parameter.animator.Play("Walk");
    }

    public void OnUpdate()
    {   

        // manual control enjoy higher priority
        if (swat.isPlayedByPlayer)
        {
            parameter.movementManager.Walk();
            return;
        }
        
        
        if (!parameter.aiManager.shouldFollow)
        {
            swat.TransitionState(SWATStateType.Idle);
            return;
        }

        parameter.movementManager.Walk(Mathf.Sign(parameter.aiManager.distance));
    }

    public void HandleInput(){
        if(Input.GetKeyDown("r")){
            swat.TransitionState(SWATStateType.Reload);
            return;
        }
        if(Input.GetKeyDown("f")){
            swat.TransitionState(SWATStateType.Melee);
            return;
        }
        if(Input.GetKey("j")){
            if(Input.GetKey("s")){
                swat.TransitionState(SWATStateType.CrouchShoot);
                return;
            }
            swat.TransitionState(SWATStateType.StandShoot);
            return;
        }
        if(!(Input.GetKey("a") || Input.GetKey("d"))){
            swat.TransitionState(SWATStateType.Idle);
            return;
        }
        if(Input.GetKey(KeyCode.LeftShift)){
            swat.TransitionState(SWATStateType.Run);
            return;
        }
        if(Input.GetKeyDown("k")){
            if(parameter.movementManager.CanJump()){
                swat.TransitionState(SWATStateType.Jump);
                return;
            }
            
        }
        if(Input.GetKey("s")){
            swat.TransitionState(SWATStateType.Crouch);
        }
    }

    public void OnExit(){

    }
}

public class SWATRunState : SurvivorIState
{
    private SWAT swat;
    
    private SWATParameter parameter;

    public SWATRunState(SWAT swat){
        this.swat = swat;

        this.parameter = swat.parameter;
    }

    public void OnEnter(){
        parameter.movementManager.isRunning = true;
        parameter.animator.Play("Run");
    }

    public void OnUpdate(){
        if (swat.isPlayedByPlayer)
        {
            parameter.movementManager.Run();
        }
        
    }

    public void HandleInput(){
        if(Input.GetKeyDown("r")){
            swat.TransitionState(SWATStateType.Reload);
            return;
        }
        if(Input.GetKeyDown("f")){
            swat.TransitionState(SWATStateType.Melee);
            return;
        }
        if(Input.GetKey("j")){
            if(Input.GetKey("s")){
                swat.TransitionState(SWATStateType.CrouchShoot);
                return;
            }
            swat.TransitionState(SWATStateType.StandShoot);
            return;

        }
        if(!Input.GetKey(KeyCode.LeftShift)){
            swat.TransitionState(SWATStateType.Walk);
            return;
        }
        if(!(Input.GetKey("a") || Input.GetKey("d"))){
            swat.TransitionState(SWATStateType.Idle);
            return;
        }
        if(Input.GetKeyDown("k")){
            if(parameter.movementManager.CanJump()){
                swat.TransitionState(SWATStateType.Jump);
                return;
            }
            
        }
        if(Input.GetKey("s")){
            swat.TransitionState(SWATStateType.Crouch);
        }
    }

    public void OnExit(){

    }
}
public class SWATJumpState : SurvivorIState
{
    private SWAT swat;
    
    private SWATParameter parameter;

    public SWATJumpState(SWAT swat){
        this.swat = swat;

        this.parameter = swat.parameter;
    }

    public void OnEnter(){
        parameter.animator.Play("Jump");
        parameter.movementManager.Jump();
    }

    public void OnUpdate(){
        if(parameter.movementManager.isInMidAir){
            return;
        }
        
        swat.TransitionState(SWATStateType.Idle);
    }

    public void HandleInput(){
        
    }

    public void OnExit(){

    }
}
public class SWATHurtState : SurvivorIState
{
    private SWAT swat;
    
    private SWATParameter parameter;

    public SWATHurtState(SWAT swat){
        this.swat = swat;

        this.parameter = swat.parameter;
    }

    public void OnEnter(){

    }

    public void OnUpdate(){

    }

    public void HandleInput(){

    }

    public void OnExit(){

    }
}
public class SWATDieState : SurvivorIState
{
    private SWAT swat;
    
    private SWATParameter parameter;

    public SWATDieState(SWAT swat){
        this.swat = swat;

        this.parameter = swat.parameter;
    }

    public void OnEnter(){

    }

    public void OnUpdate(){

    }

    public void HandleInput(){

    }

    public void OnExit(){

    }
}
public class SWATCrouchState : SurvivorIState
{
    private SWAT swat;
    
    private SWATParameter parameter;

    public SWATCrouchState(SWAT swat){
        this.swat = swat;

        this.parameter = swat.parameter;
    }

    public void OnEnter(){
        parameter.isCrouching = true;

        parameter.animator.Play("Crouch");
    }

    public void OnUpdate(){

    }

    public void HandleInput(){
        if(!Input.GetKey("s")){
            swat.TransitionState(SWATStateType.Idle);
            return;
        }
        if(Input.GetKey("j")){
            swat.TransitionState(SWATStateType.CrouchShoot);
            return;
        }
    }

    public void OnExit(){
        parameter.isCrouching = false;
    }
}

public class SWATStandShootState : SurvivorIState
{
    private SWAT swat;
    
    private SWATParameter parameter;

    public SWATStandShootState(SWAT swat){
        this.swat = swat;

        this.parameter = swat.parameter;
    }

    public void OnEnter(){
        parameter.isShooting = true;
        parameter.animator.Play("StandShoot");
        parameter.shootingManager.FireABullet(parameter.standMuzzle);
    }

    public void OnUpdate()
    {
        if (swat.isPlayedByPlayer)
        {
            return;
        }
        if (parameter.aiControl.shouldShoot)
        {
            return;
        }
        swat.TransitionState(SWATStateType.Idle);
    }

    public void HandleInput(){
        if(Input.GetKey("s")){
            swat.TransitionState(SWATStateType.CrouchShoot);
            return;
        }
        if(!Input.GetKey("j")){
            swat.TransitionState(SWATStateType.Idle);
            return;
        }
        else{
            swat.TransitionState(SWATStateType.StandShoot);
            return;
        }
    }

    public void OnExit(){
        parameter.isShooting = false;
    }
}
public class SWATCrouchShootState : SurvivorIState
{
    private SWAT swat;
    
    private SWATParameter parameter;

    public SWATCrouchShootState(SWAT swat){
        this.swat = swat;

        this.parameter = swat.parameter;
    }

    public void OnEnter(){
        parameter.isShooting = true;
        parameter.animator.Play("CrouchShoot");
        parameter.shootingManager.FireABullet(parameter.crouchMuzzle);
    }

    public void OnUpdate(){

    }

    public void HandleInput(){
        if(!Input.GetKey("s")){
            swat.TransitionState(SWATStateType.StandShoot);
            return;
        }
        if(!Input.GetKey("j")){
            swat.TransitionState(SWATStateType.Idle);
            return;
        }
        else{
            swat.TransitionState(SWATStateType.CrouchShoot);
            return;
        }
    }

    public void OnExit(){
        parameter.isShooting = false;
    }
}
public class SWATMeleeState : SurvivorIState
{
    private SWAT swat;
    
    private SWATParameter parameter;

    private AnimatorStateInfo info;

    public SWATMeleeState(SWAT swat){
        this.swat = swat;

        this.parameter = swat.parameter;
    }

    public void OnEnter(){
        parameter.isDoingMelee = true;
        parameter.animator.Play("Melee");
        info = parameter.animator.GetCurrentAnimatorStateInfo(0);
    }

    public void OnUpdate(){
        info = parameter.animator.GetCurrentAnimatorStateInfo(0);
        if(!info.IsName("Melee")){
            return;
        }
        if(info.normalizedTime > 1f){
            swat.TransitionState(SWATStateType.Idle);
            return;
        }
    }

    public void HandleInput(){

    }

    public void OnExit(){
        parameter.isDoingMelee = false;
    }
}
public class SWATReloadState : SurvivorIState
{
    private SWAT swat;
    
    private SWATParameter parameter;

    private AnimatorStateInfo info;

    public SWATReloadState(SWAT swat){
        this.swat = swat;

        this.parameter = swat.parameter;
    }

    public void OnEnter(){
        parameter.isReloading = true;
        parameter.animator.Play("Reload");
        info = parameter.animator.GetCurrentAnimatorStateInfo(0);
    }

    public void OnUpdate(){
        info = parameter.animator.GetCurrentAnimatorStateInfo(0);

        if(!info.IsName("Reload")){
            return;
        }
        if(info.normalizedTime > 1f){
            swat.TransitionState(SWATStateType.Idle);
            return;
        }
    }

    public void HandleInput(){

    }

    public void OnExit(){
        parameter.isReloading = false;
    }
}
