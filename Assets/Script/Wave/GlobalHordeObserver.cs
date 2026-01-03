using UnityEngine;
using System.Collections.Generic;
public class GlobalHordeObserver : MonoBehaviour
{
    public static List<GameObject> currentZombiesInScene = new List<GameObject>();

    public static void AddZombieToCurZombieList(GameObject zombie)
    {
        currentZombiesInScene.Add(zombie);
    }

    public static void EmptyZombieFromZombieList()
    {
        currentZombiesInScene.Clear();
    }
}
