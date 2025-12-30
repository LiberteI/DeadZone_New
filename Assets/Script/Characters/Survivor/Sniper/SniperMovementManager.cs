using UnityEngine;

public class SniperMovementManager : BaseMovementManager
{
    private Sniper sniper;

    private SniperParameter sniperParameter;

    void Awake()
    {
        sniper = (Sniper)base.survivor;

        sniperParameter = sniper.parameter;
    }

    public override bool CanJump(){
        if (!base.CanJump())
        {
            return false;
        }
        if (!sniper.isPlayedByPlayer)
        {
            return false;
        }
        if(sniperParameter.isShooting){
            return false;
        }

        if(sniperParameter.isDoingMelee){
            return false;
        }

        if(sniperParameter.isReloading){
            return false;
        }

        if(sniperParameter.isCrouching){
            return false;
        }
        
        return true;
    }

    protected override bool CanMove(){
        if (!base.CanMove())
        {
            return false;
        }
        if (!sniper.isPlayedByPlayer)
        {
            return false;
        }
        if(sniperParameter.isShooting){
            return false;
        }
        if(sniperParameter.isDoingMelee){
            return false;
        }

        if(sniperParameter.isReloading){
            return false;
        }
        if(sniperParameter.isCrouching){
            return false;
        }
        return true;
    }

    protected override bool CanFlip() {

        if (!base.CanFlip())
        {
            return false;
        }
        if(sniperParameter.isDoingMelee){
            return false;
        }
        if (!sniper.isPlayedByPlayer)
        {
            return false;
        }
        if(sniperParameter.isShooting){
            return false;
        }

        if(sniperParameter.isReloading){
            return false;
        }  
        return true;
    }
}
