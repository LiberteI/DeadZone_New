using UnityEngine;
using System.Collections.Generic;
public class GlobalHordeObserver : MonoBehaviour
{
    public static List<GameObject> currentZombiesInScene = new List<GameObject>();
    
    public static bool shouldStartObserveCurrentZombies = false;
    
    public static bool canContinueMainStream = true;

    public bool canStartSpawnZombiesCasually;

    private float casualZombieTimer = 0;
    void Update()
    {
        if (canStartSpawnZombiesCasually)
        {
            TryGenerateZombieCasually();
        }
        if (shouldStartObserveCurrentZombies)
        {
            MonitorCurrentHordeResolved();
        }

        TryToggleCanStartSpawnZombiesCasually();
        // Debug.Log($"current zombie : {currentZombiesInScene.Count}");

        // Debug.Log($"can continue: {canContinueMainStream}");
        // Debug.Log($"can spawn casual zombies: {canStartSpawnZombiesCasually}");
    }
    void OnEnable()
    {
        EventManager.OnHordeStart += SetObserverFalse;
        EventManager.OnHordeEnd += SetObserverTrue;
    }

    void OnDisable()
    {
        EventManager.OnHordeStart -= SetObserverFalse;
        EventManager.OnHordeEnd -= SetObserverTrue;
    }

    
    private void TryGenerateZombieCasually()
    {
        if(casualZombieTimer > 0)
        {
            casualZombieTimer -= Time.deltaTime;
            return;
        }
        
        GenerateCasualZombies();
        casualZombieTimer = 5f;
    }

    private void GenerateCasualZombies()
    {
        Debug.Log("generated");
        ZombieSpawner randomZombieSpawner = HordeManager.Instance.GetRandomZombieSpawner();
        randomZombieSpawner.SpawnZombiesCasually();
    }

    // start spawning zombies after first SOS
    private void TryToggleCanStartSpawnZombiesCasually()
    {
        if (canStartSpawnZombiesCasually)
        {
            return;
        }

        if (HordeManager.Instance != null && HordeManager.Instance.FirstHorde.hasTriggered)
        {
            canStartSpawnZombiesCasually = true;
        }
    }
    private void SetObserverTrue()
    {
        shouldStartObserveCurrentZombies = true;
    }

    private void SetObserverFalse()
    {
        shouldStartObserveCurrentZombies = false;
    }
    private void MonitorCurrentHordeResolved()
    {
        foreach(var zombie in currentZombiesInScene)
        {
            if(zombie == null)
            {
                continue;
            }
            ZombieHealthManager healthManager = zombie.GetComponent<ZombieHealthManager>();
            if (!healthManager.isDead)
            {
                canContinueMainStream = false;
                return;
            }
            
        }
        canContinueMainStream = true;
    }
    public static void AddZombieToCurZombieList(GameObject zombie)
    {
        currentZombiesInScene.Add(zombie);
    }

    public static void EmptyZombieFromZombieList()
    {
        currentZombiesInScene.Clear();
    }



}
