using UnityEngine;

[CreateAssetMenu(fileName = "HordeProfile", menuName = "Scriptable Objects/HordeProfile")]
public class HordeProfile : ScriptableObject
{
    [Header("General")]
    public string description;
    
    public int totalZombies;

    [Header("Zombie Ratio")]
    [Range(0, 1)] public float runnerRatio;

    [Range(0, 1)] public float clutcherRatio;

    [Range(0, 1)] public float stalkerRatio;

    [Range(0, 1)] public float boomerRatio;

    [Range(0, 1)] public float poisonerRatio;

    [Range(0, 1)] public float screamerRatio;

    [Range(0, 1)] public float tankRatio;

    [Range(0, 1)] public float jockeyRatio;


}
