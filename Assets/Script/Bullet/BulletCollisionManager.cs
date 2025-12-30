using UnityEngine;
using System;

/*
    This script is responsible for detecting collision and passing collision data
*/
[Serializable]
public class BulletHitData{
    public GameObject initiator;

    // receiver identifier
    public GameObject receiver;
    
    public float damage;

    public Vector2 bulletFlyingDir;
}
public class BulletCollisionManager : MonoBehaviour
{
    [SerializeField] private BulletManager manager;

    [SerializeField] private BulletHitData data;

    private Vector3 lastFramePos;

    private Vector3 curFramePos;

    // initiator : assign during runtime
    private GameObject initiator;

    void Start()
    {
        lastFramePos = transform.position;
    }

    void FixedUpdate()
    {
        RayCastSweep();
    }
    private void SetData(GameObject receiver)
    {
        if (receiver == null)
        {
            Debug.Log("receiver is null");
            return;
        }
        data.damage = manager.bulletDamage;

        data.bulletFlyingDir = manager.RB.linearVelocity.normalized;

        data.receiver = receiver;
        
        data.initiator = this.initiator;
        
    }

    /*
        This collision check is not reliable enough for precise hit check.

        As a result: I will implement a ray cast sweep.

        compute last frame pos and this frame pos, and do a raycast. If there is object in between, treat it as a collision.
    */
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log(other);
        if (other == null)
        {
            return;
        }
        if (!other.CompareTag("Zombie"))
        {
            return;
        }

        SetData(other.gameObject);

        EventManager.RaiseOnBulletHit(data);

        // Debug.Log($"Collider Hit {data.receiver}");

        manager.DestroyBullet();
    }

    private void RayCastSweep()
    {
        curFramePos = transform.position;

        Vector3 direction = curFramePos - lastFramePos;

        float distance = direction.magnitude;

        if (distance == 0)
        {
            lastFramePos = curFramePos;
            return;
        }


        RaycastHit2D hit = Physics2D.Raycast(lastFramePos, direction.normalized, distance);

        if (hit.collider == null)
        {
            lastFramePos = curFramePos;
            return;
        }
        
        if (!hit.collider.CompareTag("Zombie"))
        {
            lastFramePos = curFramePos;
            return;
        }
        SetData(hit.collider.gameObject);
        // Debug.Log(hit.collider.gameObject);
        EventManager.RaiseOnBulletHit(data);

        // Debug.Log($"Raycast Hit {data.receiver}");

        manager.DestroyBullet();

        lastFramePos = curFramePos;
    }

    public void SetBulletInitiator(GameObject survivor)
    {
        this.initiator = survivor;
        
    }
}
