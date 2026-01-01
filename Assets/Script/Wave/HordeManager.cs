using UnityEngine;
using System;
using System.Collections.Generic;
[Serializable]
public class ThemedHorde
{
    // Configured via inspector
    [SerializeField] private HordeProfile profile;
    [SerializeField] private string hordeName;

    // Runtime state
    public bool hasTriggered = false;
    
}

public class HordeManager : MonoBehaviour
{

    [SerializeField]
    private List<GameObject> ZombieSpawnPoints = new List<GameObject>();

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
}