using System.Collections.Generic;

using UnityEngine;


public class ZombieSpawner : MonoBehaviour
{
    [SerializeField] private float spawnRadius;

    [Header("Zombies")]

    [SerializeField] private List<GameObject> runnerPrefabs;

    [SerializeField] private List<GameObject> clutcherPrefabs;

    [SerializeField] private List<GameObject> stalkerPrefabs;

    [SerializeField] private GameObject boomerPrefab;

    [SerializeField] private GameObject tankPrefab;

    [SerializeField] private GameObject screamerPrefab;

    [SerializeField] private GameObject jockeyPrefab;

    [SerializeField] private GameObject poisonerPrefab;

    
    private GameObject GenerateZombieWithinRadius(GameObject zombie)
    {
        float newX = UnityEngine.Random.Range(transform.position.x - spawnRadius, transform.position.x + spawnRadius);

        Vector3 newSpawnPointPosition = transform.position;

        newSpawnPointPosition.x = newX;

        GameObject obj = Instantiate(zombie, newSpawnPointPosition, Quaternion.identity);

        obj.SetActive(true);

        return obj;
    }
    
    private GameObject ChooseFromZombies(List<GameObject> list)
    {
        int randomIdx = UnityEngine.Random.Range(0, list.Count);

        return list[randomIdx];
    }

    public GameObject SpawnPoisoner()
    {
        return GenerateZombieWithinRadius(poisonerPrefab);
    }
    public GameObject SpawnJockey()
    {
        return GenerateZombieWithinRadius(jockeyPrefab);
    }
    public GameObject SpawnScreamer()
    {
        return GenerateZombieWithinRadius(screamerPrefab);
    }
    public GameObject SpawnTank()
    {
        return GenerateZombieWithinRadius(tankPrefab);
    }
    public GameObject SpawnBoomer()
    {
        return GenerateZombieWithinRadius(boomerPrefab);
    }
    public GameObject SpawnRunner()
    {
        return GenerateZombieWithinRadius(ChooseFromZombies(runnerPrefabs));
    }
    
    public GameObject SpawnStalker()
    {
        return GenerateZombieWithinRadius(ChooseFromZombies(stalkerPrefabs));
    }

    public GameObject SpawnClutcher()
    {
        return GenerateZombieWithinRadius(ChooseFromZombies(clutcherPrefabs));
    }
    
    public GameObject SpawnCommonInfected()
    {
        float identifier = GlobalHelper.GetRandomNumberWithRange(0f, 100f);
        GameObject spawned;

        if(identifier < 33f)
        {
            spawned = SpawnRunner();
        }
        else if(identifier > 66f)
        {
            spawned = SpawnClutcher();
        }
        else
        {
            spawned = SpawnStalker();
        }
        GlobalHordeObserver.AddZombieToCurZombieList(spawned);
        return spawned;
    }

    public void SpawnZombiesCasually()
    {
        float identifier = GlobalHelper.GetRandomNumberWithRange(0f, 100f);

        if(identifier < 33f)
        {
            SpawnRunner();
        }
        else if(identifier > 66f)
        {
            SpawnClutcher();
        }
        else
        {
            SpawnStalker();
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }

    
}
