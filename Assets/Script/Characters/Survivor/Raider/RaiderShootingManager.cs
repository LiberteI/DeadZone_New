using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaiderShootingManager : BaseShootingManager
{
    private float curShootTimer;

    private float maxShootTime = 1f;

    // bullet count is an odd num
    public int bulletCount = 3;

    public float maxAngle = 60f;

    private float fireAnimationOffset = 0.1f;

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

    public override void FireABullet(Transform muzzle)
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

    protected override IEnumerator ExecuteFireABullet(Transform muzzle)
    {
        List<GameObject> bullets = new List<GameObject>();

        yield return new WaitForSeconds(fireAnimationOffset);
        // instantiate bullets
        for (int i = 0; i < bulletCount; i++)
        {
            bullets.Add(Instantiate(bulletPrefab, muzzle.position, Quaternion.identity));

            if (!bullets[i].GetComponent<BulletManager>())
            {
                Debug.Log("curBullet is null");
                yield break;
            }
            BulletManager curBullet = bullets[i].GetComponent<BulletManager>();

            BulletCollisionManager curBulletCollider = bullets[i].GetComponentInChildren<BulletCollisionManager>();

            curBulletCollider.SetBulletInitiator(this.gameObject);

            curBullet.isFacingRight = survivor.parameter.isFacingRight;

            bullets[i].SetActive(true);
        }

        // spread bullets
        SpreadBullets(bullets);

        yield return new WaitForSeconds(firingInterval);

        firing = null;
    }

    private void SpreadBullets(List<GameObject> bullets)
    {
        // compute mid index and half bullet count
        int midIdx = bullets.Count / 2;

        // spread angle of first bullets
        float dividedAngle = maxAngle / midIdx;

        for (int i = 0; i < bullets.Count; i++)
        {
            // compute z pos
            float zPos = -90f + (midIdx - i) * dividedAngle;

            // assign z pos
            bullets[i].transform.rotation = Quaternion.Euler(0, 0, zPos);
        }
    }
}
