using UnityEngine;

public class ShotgunBulletManager : BulletManager
{
    void Start()
    {
        curDyingTime = dyingTime;

        if(isFacingRight){
            var newEularAngles = transform.eulerAngles;
            newEularAngles.y = 0f;
            transform.eulerAngles = newEularAngles;

            RB.AddForce(transform.up * bulletForce, ForceMode2D.Impulse);
        }
        else{
            var newEularAngles = transform.eulerAngles;
            newEularAngles.y = 180f;
            transform.eulerAngles = newEularAngles;

            RB.AddForce(transform.up * bulletForce, ForceMode2D.Impulse);
        }
    }
}
