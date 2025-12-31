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
    private static List<ThemedHorde> mainStreamHordes = new List<ThemedHorde>();

    [SerializeField]
    private ThemedHorde firstHorde;

    [SerializeField]
    private ThemedHorde secondHorde;

    [SerializeField]
    private ThemedHorde thirdHorde;

    void Awake()
    {
        mainStreamHordes.Add(firstHorde);
        mainStreamHordes.Add(secondHorde);
        mainStreamHordes.Add(thirdHorde);
    }
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

        // find the first hasnt triggered horde to summon zombies
        foreach(var horde in mainStreamHordes)
        {
            if (!horde.hasTriggered)
            {
                horde.hasTriggered = true;
                Debug.Log("new horde!");
                // todo: spawn zombies to selected spawners
                break;
            }

            Debug.Log("You win!");
        }
    }

    private bool CanContinueMainStream()
    {
        
        return true;
    }
}