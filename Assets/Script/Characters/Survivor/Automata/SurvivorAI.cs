using UnityEngine;
/*
    Automatic Survivor can follow or hold position
*/

public enum AIMode
{
    Follow,

    HoldPosition
}
public class SurvivorAI : MonoBehaviour
{
    [SerializeField] private SurvivorBase survivor;

    [Header("distance > 0 : target is to the right")]
    public float distance;

    public bool shouldFollow;

    [SerializeField] private float distanceThreshold;

    public AIMode curAIMode;

    [SerializeField] private SurvivorManager survivorManager;

    public bool shouldShoot;

    [SerializeField] private GameObject prioritisedZombie;

    private float alterTimer;
    void Update()
    {
        CalculateDistanceToTarget();

        DecideShouldFollow();

        TryResetPrioritisedZombie();

        TryUpdateAlterTimer();
    }

    void OnEnable()
    {
        survivor = this.gameObject.GetComponent<SurvivorBase>();

        EventManager.OnSeeZombie += SetZombieIncomingDir;

        EventManager.OnAlterAIType += AlterAIMode;

    }
    void OnDisable()
    {
        EventManager.OnSeeZombie -= SetZombieIncomingDir;

        EventManager.OnAlterAIType -= AlterAIMode;
    }

    public void AlterAIMode(GameObject survivor)
    {
        if (alterTimer > 0)
        {
            return;
        }
        if (this.gameObject != survivor)
        {
            return;
        }
        if (curAIMode == AIMode.Follow)
        {
            curAIMode = AIMode.HoldPosition;
        }
        else
        {
            curAIMode = AIMode.Follow;
        }

        StartAlterTimer();
    }
    private void StartAlterTimer()
    {
        alterTimer = 1f;
    }

    private void TryUpdateAlterTimer()
    {
        if (alterTimer > 0)
        {
            alterTimer -= Time.deltaTime;
        }
    }

    private void SetZombieIncomingDir(DetectionData data)
    {
        if (data.initiator != this.gameObject)
        {
            return;
        }
        if (survivor.isPlayedByPlayer)
        {
            // Debug.Log("Skip process");
            return;
        }

        if (prioritisedZombie == null)
        {
            // has got a zombie to prioritise.

            prioritisedZombie = data.receiver;
        }

        // flip to the target

        float dir = prioritisedZombie.transform.position.x - data.initiator.transform.position.x;
        /*
            1. calculate zombie's relative position
        */

        // Debug.Log($"Sees zombie at {dir}");
        if (dir < 0)
        {
            survivor.parameter.isFacingRight = false
            ;
            transform.rotation = Quaternion.Euler(0, 180f, 0);
        }
        else if (dir > 0)
        {
            survivor.parameter.isFacingRight = true;

            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        shouldShoot = true;
    }

    private void CalculateDistanceToTarget()
    {
        // calculate distance on the x axis
        if (survivorManager.survivorIsPlayed == null)
        {
            Debug.Log("current survivor played is null");
            return;
        }
        // cache Transform
        Transform target = survivorManager.survivorIsPlayed.GetComponentInChildren<Rigidbody2D>().transform;

        float curDistance = target.transform.position.x - transform.position.x;

        distance = curDistance;
    }

    private void DecideShouldFollow()
    {
        if (curAIMode != AIMode.Follow)
        {
            shouldFollow = false;
            return;
        }
        if (shouldShoot)
        {
            shouldFollow = false;
            return;
        }
        if (Mathf.Abs(distance) > distanceThreshold)
        {
            shouldFollow = true;
        }
        else
        {
            shouldFollow = false;
        }
    }

    private void TryResetPrioritisedZombie()
    {
        if (prioritisedZombie == null)
        {
            shouldShoot = false;
            return;
        }
        if (!prioritisedZombie.GetComponent<ZombieHealthManager>())
        {
            Debug.Log("Health Manager is null");
            return;
        }
        ZombieHealthManager healthManager = prioritisedZombie.GetComponent<ZombieHealthManager>();

        if (healthManager.isDead)
        {
            shouldShoot = false;

            prioritisedZombie = null;
        }

    }
}
