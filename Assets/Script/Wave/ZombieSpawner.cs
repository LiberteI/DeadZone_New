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

    
    private void GenerateZombieWithinRadius(GameObject zombie)
    {
        float newX = UnityEngine.Random.Range(transform.position.x - spawnRadius, transform.position.x + spawnRadius);

        Vector3 newSpawnPointPosition = transform.position;

        newSpawnPointPosition.x = newX;

        GameObject obj = Instantiate(zombie, newSpawnPointPosition, Quaternion.identity);

        obj.SetActive(true);

    }
    
    private GameObject ChooseFromZombies(List<GameObject> list)
    {
        int randomIdx = UnityEngine.Random.Range(0, list.Count);

        return list[randomIdx];
    }

    public void SpawnPoisoner()
    {
        GenerateZombieWithinRadius(poisonerPrefab);
    }
    public void SpawnJockey()
    {
        GenerateZombieWithinRadius(jockeyPrefab);
    }
    public void SpawnScreamer()
    {
        GenerateZombieWithinRadius(screamerPrefab);
    }
    public void SpawnTank()
    {
        GenerateZombieWithinRadius(tankPrefab);
    }
    public void SpawnBoomer()
    {
        GenerateZombieWithinRadius(boomerPrefab);
    }
    public void SpawnRunner()
    {
        GenerateZombieWithinRadius(ChooseFromZombies(runnerPrefabs));
    }
    
    public void SpawnStalker()
    {
        GenerateZombieWithinRadius(ChooseFromZombies(stalkerPrefabs));
    }

    public void SpawnClutcher()
    {
        GenerateZombieWithinRadius(ChooseFromZombies(clutcherPrefabs));
    }
    
    public void SpawnCommonInfected()
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
