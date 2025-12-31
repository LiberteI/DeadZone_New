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

    private void SpawnPoisoner()
    {
        GenerateZombieWithinRadius(poisonerPrefab);
    }
    private void SpawnJockey()
    {
        GenerateZombieWithinRadius(jockeyPrefab);
    }
    private void SpawnScreamer()
    {
        GenerateZombieWithinRadius(screamerPrefab);
    }
    private void SpawnTank()
    {
        GenerateZombieWithinRadius(tankPrefab);
    }
    private void SpawnBoomer()
    {
        GenerateZombieWithinRadius(boomerPrefab);
    }
    private void SpawnRunner()
    {
        GenerateZombieWithinRadius(ChooseFromZombies(runnerPrefabs));
    }
    
    private void SpawnStalker()
    {
        GenerateZombieWithinRadius(ChooseFromZombies(stalkerPrefabs));
    }

    private void SpawnClutcher()
    {
        GenerateZombieWithinRadius(ChooseFromZombies(clutcherPrefabs));
    }
    
    

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }

    
}
