using System.Collections.Generic;
using UnityEngine;

public class ZombieDetector : MonoBehaviour
{
    /*
        when survivor is not played by human, survivor should detect zombies on his own.

        Survivor has 2 triggered colliders, front and back.

        Front collider is longer, while back is shorter.

        1. front detects & back does not: shoot

        2. front does not & back does : flip

        3. front and back both do : shoot
    */
    [SerializeField] private DetectionData data;

    private List<GameObject> gameObjects;

    /*
        Flow:
        1. detect zombie : 
            if at least one zombie detected, raise OnSeeZombie with data

        2. calculate the related position of the zombie. flip based on the related position.

        3. keep shooting until the zombie is eliminated. then cache the next zombie if seen.
    */
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other == null)
        {
            return;
        }
        data.receiver = other.gameObject;

        if (!other.CompareTag("Zombie"))
        {
            return;
        }

        EventManager.RaiseOnSeeZombie(data);
    }

}
