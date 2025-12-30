using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class BaseShootingManager : MonoBehaviour
{
    /*
        This script is responsible for shooting, 
            including generating bullets, 
            managing reload etc.
    */
    [SerializeField] protected SurvivorBase survivor;

    [SerializeField] protected GameObject bulletPrefab;

    [SerializeField] protected float firingInterval; // 0 - 1

    protected Coroutine firing;

    [SerializeField] protected float shootNoise;

    public virtual void FireABullet(Transform muzzle)
    {
        if (firing != null)
        {
            return;
        }
        if (bulletPrefab == null)
        {
            Debug.Log("Bullet prefab is not assigned");
            return;
        }
        // if get called, start a coroutine
        firing = StartCoroutine(ExecuteFireABullet(muzzle));
    }

    protected virtual IEnumerator ExecuteFireABullet(Transform muzzle)
    {
        // instantiate new bullet
        GameObject bullet = Instantiate(bulletPrefab, muzzle.position, Quaternion.identity);

        BulletManager curBullet = bullet.GetComponent<BulletManager>();

        BulletCollisionManager curBulletCollider = bullet.GetComponentInChildren<BulletCollisionManager>();

        curBulletCollider.SetBulletInitiator(this.gameObject);

        if (curBullet == null)
        {
            Debug.Log("curBullet is null");
            yield break;
        }
        // determine bullet dir

        curBullet.isFacingRight = survivor.parameter.isFacingRight;

        bullet.SetActive(true);

        // announce gun shot
        EventManager.RaiseOnGunShot(shootNoise);

        yield return new WaitForSeconds(firingInterval);
        firing = null;
    }
    
    
}
