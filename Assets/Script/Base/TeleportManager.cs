using System.Collections.Generic;
using UnityEngine;

/*
    Player could dynamically choose whether or not to enter door. *

    Survivors should not use door by themselves.
    
    if survivors are in follow mode and they are in different level from the player they choose which door to use.

    If zombies are after the player then they could pass the door without getting teleported.

    
*/
public class TeleportManager : MonoBehaviour
{
    public Transform destination;

    public bool shouldIncrementLevelSignature;

    private HashSet<GameObject> blockedItems = new HashSet<GameObject>();

    private void OnTriggerExit2D(Collider2D other)
    {
        RecoverGameObject(other.gameObject);
    }

    public void BlockGameObject(GameObject obj)
    {
        blockedItems.Add(obj);
    }

    public void RecoverGameObject(GameObject obj)
    {
        blockedItems.Remove(obj);
    }

    public void Teleport(GameObject obj)
    {
        if (!ShouldTeleport(obj))
        {
            return;
        }
        obj.transform.position = destination.transform.position;

        destination.GetComponent<TeleportManager>().BlockGameObject(obj);

        EventManager.RaiseOnFloorChanged(obj, shouldIncrementLevelSignature);

        // Debug.Log($"Raise OnFloorChange for {obj}, should increment: {shouldIncrementLevelSignature}");
    }

    public bool ShouldTeleport(GameObject obj)
    {
        return !blockedItems.Contains(obj);
    }
}
