using UnityEngine;

/*
    Context: 
        1. There are 3 floors in the game scene. *
        2. Base is on the third. the second floor is basically a corridor consists of 2 doors. *
        3. The ground floor is a space outside the building. *


    Default target: 20% zombies look for base. 20% zombies search for the nearest survivor. *
    
    while in ground / second floor: *
        if zombie is closer to survivor than the door to the next floor: aggro = survivor (in the same floor)
        otherwise: aggro = door

    while in third floor: *
        aggro = base

    Retarget every ~2s (throttled scan). *

    OnHit override: if the zombie is damaged, force target = attacker for a short “stickiness” window (e.g., 3–5s).*

    Base priority: if no living allies in range, or if it’s in “siege mode,”(20% is in siege mode by default) go for base. *
*/
public class ZombieAggroManager : MonoBehaviour
{

    [SerializeField] BaseZombie zombie;

    public GameObject currentTarget;

    public Vector3 targetPos;

    public float curDistanceToTarget;

    private GameObject currentSurvivor;

    [SerializeField] private float maxAggroSwitchTimer = 2f;

    [SerializeField] private float curAggroSwitchTimer;

    [SerializeField] private float maxForceAggroTimer = 5f;

    [SerializeField] private float curForceAggroTimer;

    // fixed target on the base
    public bool isInSiegeMode;

    public int curFloor = 1;

    private GameObject gameObjToTeleport;
    void OnEnable()
    {
        gameObjToTeleport = this.gameObject;

        EventManager.OnBulletHit += ForceSwitchAggro;

        EventManager.OnMeleeHit += ForceSwitchAggro;

        EventManager.OnFloorChanged += UpdateCurFloor;
    }

    void OnDisable()
    {
        EventManager.OnBulletHit -= ForceSwitchAggro;

        EventManager.OnMeleeHit -= ForceSwitchAggro;

        EventManager.OnFloorChanged -= UpdateCurFloor;
    }
    void Update()
    {
        if (zombie.isDead)
        {
            return;
        }
        CalculateDistToBaseObj();

        CalculateDistToSurvivor();

        TryAssignAggroTarget();

        UpdateAggroSwitchTimer();

        UpdateForceAggroTimer();

        CalculateCurDistance();

        TryFindPathForSiegeMode();

        MoniterPlayerTarget();
    }

    void Start()
    {
        DecideSiegeMode();

        CalculateDistToBaseObj();

        CalculateDistToSurvivor();

        TryAssignAggroTarget();
    }
    private void UpdateCurFloor(GameObject obj, bool shouldIncrement)
    {
        if (obj != this.gameObject)
        {
            return;
        }

        if (shouldIncrement)
        {
            curFloor += 1;
        }
        else
        {
            curFloor -= 1;
        }
    }
    private void TryAssignAggroTarget()
    {
        if (!ShouldSearchForNextTarget())
        {
            return;
        }

        if (curFloor == 3)
        {
            currentTarget = BaseManager.Instance.baseObj;
            return;
        }

        if (CalculateDistToBaseObj() <= CalculateDistToSurvivor())
        {
            if (curFloor == 1)
            {
                currentTarget = BaseManager.Instance.level1Door.gameObject;

            }
            else if (curFloor == 2)
            {
                currentTarget = BaseManager.Instance.level2Door.gameObject;
            }
        }
        else
        {
            currentTarget = currentSurvivor;
        }



        curAggroSwitchTimer = maxAggroSwitchTimer;
    }

    private void CalculateCurDistance()
    {
        if (currentTarget == null)
        {
            // Debug.Log("Current target is null. Cannot calculate distance");
            return;
        }
        // Debug.Log(currentTarget);

        targetPos = currentTarget.GetComponentInChildren<Rigidbody2D>().transform.position;

        curDistanceToTarget = Mathf.Abs(targetPos.x - transform.position.x);
    }
    private float CalculateDistToBaseObj()
    {
        if (BaseManager.Instance == null)
        {
            Debug.Log("BaseManager.Instance is null");
        }

        if (curFloor == 1)
        {
            return Mathf.Abs(BaseManager.Instance.level1Door.position.x - transform.position.x);
        }
        else if (curFloor == 2)
        {
            return Mathf.Abs(BaseManager.Instance.level2Door.position.x - transform.position.x);
        }
        else
        {
            return Mathf.Abs(BaseManager.Instance.baseObj.transform.position.x - transform.position.x);
        }
    }

    private float CalculateDistToSurvivor()
    {
        currentSurvivor = FindTheNearestSurvivor(curFloor);

        if (currentSurvivor == null)
        {
            return float.PositiveInfinity;
        }
        return Mathf.Abs(currentSurvivor.GetComponentInChildren<Rigidbody2D>().transform.position.x - transform.position.x);
    }

    private GameObject FindTheNearestSurvivor(int curFloor)
    {
        /*
            Loop through survivor list and find the nearest survivor within the same floor
        */

        // distance to survivor
        float minDistance = float.PositiveInfinity;

        int survivorTargetIdx = -99;

        if (SurvivorManager.survivorList == null)
        {
            Debug.Log("survivor list not found");
            return null;
        }
        if (SurvivorManager.survivorList.Count < 1)
        {
            Debug.Log("Survivor list is empty now");

            return null;

        }
        for (int i = 0; i < SurvivorManager.survivorList.Count; i++)
        {
            float curDistance = (transform.position - SurvivorManager.survivorList[i].GetComponentInChildren<Rigidbody2D>().transform.position).sqrMagnitude;
            // Debug.Log($"Survivor Transform position: {SurvivorManager.survivorList[i].GetComponentInChildren<Rigidbody2D>().transform.position}");
            if (SurvivorManager.survivorList[i].GetComponentInChildren<SurvivorBase>().curFloor == curFloor)
            {
                if (curDistance < minDistance)
                {
                    minDistance = curDistance;

                    survivorTargetIdx = i;
                }
            }
        }
        // no survivor in the same floor found
        if (survivorTargetIdx == -99)
        {
            // Debug.Log("no survivor in the same floor found");
            return null;
        }
        else
        {
            return SurvivorManager.survivorList[survivorTargetIdx];
        }
    }

    private bool ShouldSearchForNextTarget()
    {
        if (isInSiegeMode)
        {
            return false;
        }
        if (curForceAggroTimer > 0)
        {
            return false;
        }

        if (curAggroSwitchTimer > 0)
        {
            return false;
        }
        return true;
    }
    private void UpdateAggroSwitchTimer()
    {
        if (curAggroSwitchTimer > 0)
        {
            curAggroSwitchTimer -= Time.deltaTime;
        }
    }

    private void UpdateForceAggroTimer()
    {
        if (curForceAggroTimer > 0)
        {
            curForceAggroTimer -= Time.deltaTime;
        }
    }



    // aggro overrider
    private void ForceSwitchAggro(BulletHitData data)
    {
        if (data.receiver != this.gameObject)
        {
            return;
        }

        currentTarget = data.initiator;

        curForceAggroTimer = maxForceAggroTimer;
    }

    private void ForceSwitchAggro(MeleeHitData data)
    {
        if (data.receiver != this.gameObject)
        {
            return;
        }

        // stick to target for force aggro time
        currentTarget = data.initiator;

        curForceAggroTimer = maxForceAggroTimer;
    }

    private void DecideSiegeMode()
    {
        float random = UnityEngine.Random.Range(0, 100);

        if (random > 80f)
        {
            isInSiegeMode = true;
        }
        else
        {
            isInSiegeMode = false;
        }
    }

    private void TryFindPathForSiegeMode()
    {
        if (!isInSiegeMode)
        {
            return;
        }
        if (curForceAggroTimer > 0)
        {
            return;
        }

        if (curFloor == 1)
        {
            currentTarget = BaseManager.Instance.level1Door.gameObject;
        }
        else if (curFloor == 2)
        {
            currentTarget = BaseManager.Instance.level2Door.gameObject;
        }
        else
        {
            currentTarget = BaseManager.Instance.baseObj;
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other == null)
        {
            return;
        }
        if (!other.CompareTag("Door"))
        {
            return;
        }
        
        TeleportManager door = other.GetComponent<TeleportManager>();
       
        // skip if currentTarget is player
        if (currentTarget == BaseManager.Instance.level1Door.gameObject || currentTarget == BaseManager.Instance.level2Door.gameObject)
        {
            
            door.Teleport(gameObjToTeleport);
            if (curFloor == 2)
            {
                currentTarget = BaseManager.Instance.level2Door.gameObject;
            }
            else if (curFloor == 3)
            {
                currentTarget = BaseManager.Instance.baseObj;
            }
        }

    }

    private void MoniterPlayerTarget()
    {
        // compensate when player is not in the same level

        if (currentTarget.GetComponent<SurvivorBase>())
        {
            // if current following a player
            if (currentTarget.GetComponent<SurvivorBase>().curFloor != curFloor)
            {
                // stick to the floor-based target
                if (curFloor == 2)
                {
                    currentTarget = BaseManager.Instance.level2Door.gameObject;
                }
                else if (curFloor == 3)
                {
                    currentTarget = BaseManager.Instance.baseObj;
                }
                else if(curFloor == 1)
                {
                    currentTarget = BaseManager.Instance.level1Door.gameObject;
                }
            }
        }
    }
}
