using UnityEngine;
using System;
[Serializable]
public class ThemedHorde
{
    // Configured via inspector
    [SerializeField] private HordeProfile profile;
    [SerializeField] private string hordeName;

    // Runtime state
    private bool hasTriggered;

}

public class HordeManager : MonoBehaviour
{
    
}