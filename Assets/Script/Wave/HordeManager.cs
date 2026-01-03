using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class ThemedHorde
{
    // Configured via inspector
    public HordeProfile profile;

    // Runtime state
    public bool hasTriggered = false;
    
}

public class HordeManager : MonoBehaviour
{

    

    [SerializeField]
    private List<GameObject> zombieSpawnPoints = new List<GameObject>();

    [SerializeField]
    private ThemedHorde firstHorde;

    [SerializeField]
    private ThemedHorde secondHorde;

    [SerializeField]
    private ThemedHorde thirdHorde;

    public bool canStartSpawnZombiesCasually;

    void OnEnable()
    {
        EventManager.OnSendMessage += TryTriggerMainStreamHorde;

    }

    void Disable()
    {
        EventManager.OnSendMessage -= TryTriggerMainStreamHorde;
    }

    private void TryTriggerMainStreamHorde()
    {
        
        if (!CanContinueMainStream())
        {
            return;
        }

        if (!firstHorde.hasTriggered)
        {
            TryToggleCanStartSpawnZombiesCasually();
            // trigger first horde
            StartCoroutine(StartHordeOne(firstHorde, 20));
            return;
        }

        if (!secondHorde.hasTriggered)
        {
            // trigger second horde
            return;
        }

        if (!thirdHorde.hasTriggered)
        {
            // trigger third horde
            return;
        }

        // trigger the last infinite horde before escaping
    }

    // start spawning zombies after first SOS
    private void TryToggleCanStartSpawnZombiesCasually()
    {
        if (canStartSpawnZombiesCasually)
        {
            return;
        }

        if (firstHorde.hasTriggered)
        {
            canStartSpawnZombiesCasually = true;
        }
    }
    private bool CanContinueMainStream()
    {
        
        return true;
    }

    // first horde
    // randomly assign spawners to spawn zombies. spawn 1-3 zombies per time. spawn zombies every 0.5-1 second
    // resolve horde after all the zombies are killed

    private IEnumerator StartHordeOne(ThemedHorde currentHorde, int hordeIteration)
    {   
        int zombieCount = currentHorde.profile.totalZombies;
        
        for(int i = 0; i < hordeIteration; i++)
        {
            int zombieCountToSpawn = GlobalHelper.GetRandomNumberWithRange(1, 3);
            
            int selectedZombieSpawnPointIndex = GlobalHelper.GetRandomNumberWithRange(0, zombieSpawnPoints.Count - 1);
            GameObject selectedZombieSpawnPoint = zombieSpawnPoints[selectedZombieSpawnPointIndex];

            ZombieSpawner selectedZombieSpawner = selectedZombieSpawnPoint.GetComponent<ZombieSpawner>();
            // generate zombies using selected zombie spawner

            for(int j = 0; j < zombieCountToSpawn; j++)
            {
                selectedZombieSpawner.SpawnCommonInfected();
            }

            yield return GlobalHelper.GetRandomNumberWithRange(0.5f, 1f);
        }

        
        yield return null;
    }
}
