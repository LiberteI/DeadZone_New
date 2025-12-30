using UnityEngine;

public class RaiderMovementManager : BaseMovementManager
{
    private Raider raider;

    private RaiderParameter raiderParameter;

    void Awake()
    {
        raider = (Raider)base.survivor;

        raiderParameter = raider.parameter;
    }

    public override bool CanJump(){
        if (!base.CanJump())
        {
            return false;
        }
        if (!raider.isPlayedByPlayer)
        {
            return false;
        }
        if(raiderParameter.isShooting){
            return false;
        }

        if(raiderParameter.isDoingMelee){
            return false;
        }

        if(raiderParameter.isReloading){
            return false;
        }

        if(raiderParameter.isCrouching){
            return false;
        }
        
        return true;
    }

    protected override bool CanMove(){
        if (!base.CanMove())
        {
            return false;
        }
        if (!raider.isPlayedByPlayer)
        {
            return false;
        }
        if(raiderParameter.isShooting){
            return false;
        }
        if(raiderParameter.isDoingMelee){
            return false;
        }

        if(raiderParameter.isReloading){
            return false;
        }
        if(raiderParameter.isCrouching){
            return false;
        }
        return true;
    }

    protected override bool CanFlip() {

        if (!base.CanFlip())
        {
            return false;
        }
        if(raiderParameter.isDoingMelee){
            return false;
        }
        if (!raider.isPlayedByPlayer)
        {
            return false;
        }
        if(raiderParameter.isShooting){
            return false;
        }

        if(raiderParameter.isReloading){
            return false;
        }  
        return true;
    }
}
