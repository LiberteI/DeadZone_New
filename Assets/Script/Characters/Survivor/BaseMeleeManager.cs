using UnityEngine;

public class BaseMeleeManager : MonoBehaviour
{
    [SerializeField] protected SurvivorBase survivor;

    [SerializeField] private MeleeHitBoxManager hitboxManager;

    [SerializeField] private float damage;

    void Update(){
        SetData();
    }

    protected void SetData(){
        if(survivor.parameter.isFacingRight){
            hitboxManager.SetData(damage, transform.right);
        }
        else{
            hitboxManager.SetData(damage, -transform.right);
        }
    }
}
