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
    public static HordeManager Instance { get; private set; }

    [SerializeField]
    private List<GameObject> zombieSpawnPoints = new List<GameObject>();

    public List<GameObject> ZombieSpawnPoints => zombieSpawnPoints;

    [SerializeField]
    private ThemedHorde firstHorde;

    [SerializeField]
    private ThemedHorde secondHorde;

    [SerializeField]
    private ThemedHorde thirdHorde;

    public ThemedHorde FirstHorde => firstHorde;

    void OnEnable()
    {
        Instance = this;
        HordeEvents.OnSendMessage += TryTriggerMainStreamHorde;

    }

    void OnDisable()
    {
        if (Instance == this)
        {
            Instance = null;
        }
        HordeEvents.OnSendMessage -= TryTriggerMainStreamHorde;
    }
    


    private void TryTriggerMainStreamHorde()
    {

        if (!GlobalHordeObserver.canContinueMainStream)
        {   
            Debug.Log("You cannot start a new horde now!");
            return;
        }

        if (!firstHorde.hasTriggered)
        {
            // Debug.Log("horde 1 triggered");
            
            // trigger first horde
            StartCoroutine(StartHordeOne(20));
            firstHorde.hasTriggered = true;

            return;
        }
        

        if (!secondHorde.hasTriggered)
        {
            Debug.Log("sencond horde");
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

    public ZombieSpawner GetRandomZombieSpawner()
    {
        int selectedZombieSpawnPointIndex = GlobalHelper.GetRandomNumberWithRange(0, zombieSpawnPoints.Count - 1);
        GameObject selectedZombieSpawnPoint = zombieSpawnPoints[selectedZombieSpawnPointIndex];

        return selectedZombieSpawnPoint.GetComponent<ZombieSpawner>();
    }

    // first horde
    // randomly assign spawners to spawn zombies. spawn 1-3 zombies per time. spawn zombies every 0.5-1 second
    // resolve horde after all the zombies are killed

    private IEnumerator StartHordeOne(int hordeIteration)
    {   
        HordeEvents.RaiseHordeStart();
        
        for(int i = 0; i < hordeIteration; i++)
        {
            int zombieCountToSpawn = GlobalHelper.GetRandomNumberWithRange(1, 3);
            
            ZombieSpawner selectedZombieSpawner = GetRandomZombieSpawner();
            // generate zombies using selected zombie spawner

            for(int j = 0; j < zombieCountToSpawn; j++)
            {
                selectedZombieSpawner.SpawnCommonInfected();
            }

            yield return GlobalHelper.GetRandomNumberWithRange(1f, 2f);
        }

        HordeEvents.RaiseHordeEnd();
        
        yield return null;
    }

    private IEnumerator StartHordeTwo(ThemedHorde secondHorde)
    {
        
        yield return null;
    }
}
