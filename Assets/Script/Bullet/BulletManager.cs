using UnityEngine;

public class BulletManager : MonoBehaviour
{
    /*
        This script is responsible for 
        1. Keeping bullet data, such as flying time, flying speed, special effects etc.
    */
    [SerializeField] protected float curDyingTime;

    [SerializeField] protected float dyingTime;

    [SerializeField] protected float bulletForce;

    public float bulletDamage;

    public Rigidbody2D RB;

    public bool isFacingRight;

    void Start(){
        curDyingTime = dyingTime;

        if(isFacingRight){
            transform.rotation = Quaternion.Euler(0, 0, -90f);
            RB.AddForce(new Vector2(1 * bulletForce, 0), ForceMode2D.Impulse);
        }
        else{
            transform.rotation = Quaternion.Euler(0, -180f, -90f);
            RB.AddForce(new Vector2(-1 * bulletForce, 0), ForceMode2D.Impulse);
        }
    }
    
    void FixedUpdate(){
        UpdateDyingTime();
    }

    private void UpdateDyingTime(){
        if(curDyingTime > 0){
            curDyingTime -= Time.deltaTime;
            return;
        }
        DestroyBullet();
    }

    public void DestroyBullet(){
        // destroy bullet
        Destroy(this.gameObject);
    }
}
