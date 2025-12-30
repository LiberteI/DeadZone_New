using UnityEngine;

public class ZombieMeleeManager : MonoBehaviour
{
    [SerializeField] private BaseZombie zombie;

    [SerializeField] private float damage;

    [SerializeField] private MeleeHitBoxManager hitboxManager;

    [SerializeField] private Transform rangeCentre;

    [SerializeField] private float rangeRadius;

    [SerializeField] private LayerMask targetLayer;

    void Update(){

        if (zombie.isDead)
        {
            return;
        }
        
        SetData();
    }

    private void OnDrawGizmosSelected(){
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(rangeCentre.position, rangeRadius);
    }

    private void SetData(){
        if (zombie.parameter == null)
        {
            Debug.Log("parameter is null");
            return;
        }
        if (zombie.parameter.isFacingRight)
        {
            hitboxManager.SetData(damage, transform.right);
        }
        else
        {
            hitboxManager.SetData(damage, -transform.right);
        }
    }

    public bool SurvivorIsInRange(){
        return Physics2D.OverlapCircle(rangeCentre.position, rangeRadius, targetLayer);
    }
}
