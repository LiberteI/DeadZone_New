using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class ThemedHorde
{
    // Configured via inspector
    [SerializeField] private HordeProfile profile;
    [SerializeField] private string hordeName;

    // Runtime state
    private bool hasTriggered;

}
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

    [SerializeField] private ThemedHorde firstHorde;

    [SerializeField] private ThemedHorde secondHorde;

    [SerializeField] private ThemedHorde thirdHorde;

    private float zombieSpawnTimer = 0;

    private float smallHordeTimer;

    private float grandHordeTimer;

    void OnEnable() 
    {
        EventManager.OnMiniHorde += SpawnMiniWave;

        EventManager.OnSmallHorde += SpawnSmallHorde;

        EventManager.OnGrandHorde += SpawnGrandHorde;

        EventManager.OnSendMessage += SpawnMainStreamHorde;
    }
    void OnDisable()
    {
        EventManager.OnMiniHorde -= SpawnMiniWave;

        EventManager.OnSmallHorde -= SpawnSmallHorde;

        EventManager.OnGrandHorde -= SpawnGrandHorde;

        EventManager.OnSendMessage -= SpawnMainStreamHorde;
    }
    void Update()
    {
        // TryGenerateCasualZombies();

        TryUpdateSmallHordeTimer();

        TryUpdateGrandHordeTimer();
    }

    private void TryGenerateCasualZombies()
    {
        if (zombieSpawnTimer > 0)
        {
            zombieSpawnTimer -= Time.deltaTime;

            return;
        }

        zombieSpawnTimer = UnityEngine.Random.Range(5f, 15f);

        SpawnCasualZombies();
    }

    private void SpawnCasualZombies()
    {
        int count = UnityEngine.Random.Range(1, 5);

        for (int i = 0; i < count; i++)
        {
            bool shouldGenerateRunner = UnityEngine.Random.Range(0f, 10f) > 4f;

            if (shouldGenerateRunner)
            {
                GenerateZombieWithinRadius(ChooseFromZombies(runnerPrefabs));
            }
            else
            {
                GenerateZombieWithinRadius(ChooseFromZombies(clutcherPrefabs));
            }
        }

    }

    private void SpawnMiniWave()
    {
        /*
            Composition: 5 - 12 zombies
            
            runners and clutchers

            random 1 elite except for tank
        */
        int count = UnityEngine.Random.Range(5, 12);

        for (int i = 0; i < count; i++)
        {
            bool shouldGenerateRunner = UnityEngine.Random.Range(0f, 10f) > 4f;

            if (shouldGenerateRunner)
            {
                GenerateZombieWithinRadius(ChooseFromZombies(runnerPrefabs));
            }
            else
            {
                GenerateZombieWithinRadius(ChooseFromZombies(clutcherPrefabs));
            }
        }

        GenerateZombieWithinRadius(ChooseFromElites());
    }

    private void SpawnSmallHorde()
    {
        /*
            Composition: 15 - 30 zombies
            
            runners and clutchers and stalkers. 3 - 5 elites

        */
        if (smallHordeTimer > 0)
        {
            return;
        }
        int count = UnityEngine.Random.Range(3, 5);

        for (int i = 0; i < count; i++)
        {
            float randomRange = UnityEngine.Random.Range(0f, 10f);

            if (randomRange < 5f)
            {
                GenerateZombieWithinRadius(ChooseFromZombies(runnerPrefabs));
            }
            else if (randomRange > 8f)
            {
                GenerateZombieWithinRadius(ChooseFromZombies(stalkerPrefabs));
            }
            else
            {
                GenerateZombieWithinRadius(ChooseFromZombies(clutcherPrefabs));
            }
        }
        int eliteCount = UnityEngine.Random.Range(3, 6);

        for (int i = 0; i < eliteCount; i++)
        {
            GenerateZombieWithinRadius(ChooseFromElites());
        }

        smallHordeTimer = 60f;
    }

    private void SpawnGrandHorde()
    {
        /*
            Composition: 40 - 60 zombies
            
            runners and clutchers and stalkers. 

            6 - 12 elites
        */

        if (grandHordeTimer > 0)
        {
            return;
        }
        int count = UnityEngine.Random.Range(40, 60);

        for (int i = 0; i < count; i++)
        {
            float randomRange = UnityEngine.Random.Range(0f, 10f);

            if (randomRange < 5f)
            {
                GenerateZombieWithinRadius(ChooseFromZombies(runnerPrefabs));
            }
            else if (randomRange > 8f)
            {
                GenerateZombieWithinRadius(ChooseFromZombies(stalkerPrefabs));
            }
            else
            {
                GenerateZombieWithinRadius(ChooseFromZombies(clutcherPrefabs));
            }
        }
        int eliteCount = UnityEngine.Random.Range(6, 13);

        for (int i = 0; i < eliteCount; i++)
        {
            GenerateZombieWithinRadius(ChooseFromElites());
        }

        GenerateZombieWithinRadius(tankPrefab);

        grandHordeTimer = 60f;
    }

    private GameObject ChooseFromElites()
    {
        float randomRange = UnityEngine.Random.Range(0f, 10f);

        if (randomRange <= 2f)
        {
            return ChooseFromZombies(stalkerPrefabs);
        }
        else if (randomRange > 2f && randomRange <= 4f)
        {
            return boomerPrefab;
        }
        else if (randomRange > 4f && randomRange <= 6f)
        {
            return poisonerPrefab;
        }
        else if (randomRange > 6f && randomRange <= 8f)
        {
            return jockeyPrefab;
        }
        else
        {
            return screamerPrefab;
        }
    }
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }

    private void TryUpdateSmallHordeTimer()
    {
        if (smallHordeTimer > 0)
        {
            smallHordeTimer -= Time.deltaTime;
        }
    }

    private void TryUpdateGrandHordeTimer()
    {
        if (grandHordeTimer > 0)
        {
            grandHordeTimer -= Time.deltaTime;
        }
    }

    private void SpawnMainStreamHorde()
    {
        /*
            1. loop through themed horde dictionary. (scriptable obj : bool)

            2. find a random themed with a triggered == false

            3. There are 4 waves before being rescued

            4. mark the horde to false to prevent future function call
        */
    }   
}
