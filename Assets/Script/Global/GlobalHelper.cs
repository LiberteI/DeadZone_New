using UnityEngine;

using System.Collections.Generic;
public static class GlobalHelper
{
    public static List<GameObject> currentZombiesInScene = new List<GameObject>();
    public static float GetRandomNumberWithRange(float smaller, float larger)
    {   
        if(smaller > larger)
        {
            Debug.LogError("Range Error!");
            return larger;
        }
        return UnityEngine.Random.Range(smaller, larger);
    }

    public static int GetRandomNumberWithRange(int smaller, int larger)
    {   
        if(smaller > larger)
        {
            Debug.LogError("Range Error!");
            return larger;
        }
        return UnityEngine.Random.Range(smaller, larger);
    }

    public static void AddZombieToCurZombieList(GameObject zombie)
    {
        currentZombiesInScene.Add(zombie);
    }

    public static void EmptyZombieFromZombieList()
    {
        currentZombiesInScene.Clear();
    }
}
