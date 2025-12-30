using UnityEngine;

public class NZIdleState : IState
{   
    private NormalZombie zombie;

    private NormalZombieParameter parameter;

    public NZIdleState(NormalZombie zombie){
        this.zombie = zombie;

        this.parameter = zombie.parameter;
    }
    public void OnEnter(){
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
            zombie.TransitionState(NormalZombieStateTypes.Attack);
        }
        zombie.TransitionState(NormalZombieStateTypes.Walk);
    }
    public void OnExit(){

    }
}

public class NZWalkState : IState
{   
    private NormalZombie zombie;

    private NormalZombieParameter parameter;

    public NZWalkState(NormalZombie zombie){
        this.zombie = zombie;

        this.parameter = zombie.parameter;
    }
    public void OnEnter(){
        parameter.animator.Play("Walk");
    }
    public void OnUpdate()
    {
        parameter.movementManager.Move();
        if (parameter.meleeManager.SurvivorIsInRange())
        {
            zombie.TransitionState(NormalZombieStateTypes.Attack);
            return;
        }
        if (parameter.aggroManager.currentTarget == null)
        {
            zombie.TransitionState(NormalZombieStateTypes.Idle);
            return;
        }

    }
    public void OnExit(){

    }
}
public class NZHurtState : IState
{   
    private NormalZombie zombie;

    private NormalZombieParameter parameter;

    public NZHurtState(NormalZombie zombie){
        this.zombie = zombie;

        this.parameter = zombie.parameter;
    }
    public void OnEnter(){

    }
    public void OnUpdate(){

    }
    public void OnExit(){

    }
}
public class NZDieState : IState
{   
    private NormalZombie zombie;

    private NormalZombieParameter parameter;

    public NZDieState(NormalZombie zombie){
        this.zombie = zombie;

        this.parameter = zombie.parameter;
    }
    public void OnEnter(){
        zombie.gameObject.tag = "Corpse";
        
        zombie.isDead = true;

        parameter.movementManager.DisableLinearVelocity();

        parameter.animator.Play("Die");
    }
    public void OnUpdate(){
        parameter.animator.Play("Die");
    }   
    public void OnExit(){

    }
}
public class NZAttackState : IState
{   
    private NormalZombie zombie;

    private NormalZombieParameter parameter;

    private AnimatorStateInfo info;

    public NZAttackState(NormalZombie zombie){
        this.zombie = zombie;

        this.parameter = zombie.parameter;
    }
    public void OnEnter(){
        parameter.movementManager.DisableLinearVelocity();

        parameter.animator.Play("Attack");
    }
    public void OnUpdate(){
        info = parameter.animator.GetCurrentAnimatorStateInfo(0);
        if(!info.IsName("Attack")){
            return;
        }
        if(info.normalizedTime > 1f){
            zombie.TransitionState(NormalZombieStateTypes.Idle);
            return;
        }
    }
    public void OnExit(){

    }
}


