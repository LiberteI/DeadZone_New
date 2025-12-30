using UnityEngine;

public class SniperShootingManager : BaseShootingManager
{
    private float curShootTimer;

    private float maxShootTime = 1f;

    void Update()
    {
        UpdateShootTimer();
    }
    public bool CanShoot()
    {
        if (curShootTimer > 0)
        {
            return false;
        }

        return true;
    }

    private void UpdateShootTimer()
    {
        if (curShootTimer > 0)
        {
            curShootTimer -= Time.deltaTime;
        }
    }

    public void SetShootTimer()
    {
        curShootTimer = maxShootTime;
    }

    public override void FireABullet(Transform muzzle){
        if(firing != null){
            return;
        }
        if(bulletPrefab == null){
            Debug.Log("Bullet prefab is not assigned");
            return;
        }
        // if get called, start a coroutine
        firing = StartCoroutine(ExecuteFireABullet(muzzle));
    }
}
