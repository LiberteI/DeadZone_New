using UnityEngine;
using System;

[Serializable]
public class ClutchData
{
    public GameObject initiator;

    public GameObject receiver;

}
public class ClutchBoxManager : MonoBehaviour
{
    public ClutchData data;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other == null)
        {
            return;
        }

        data.receiver = other.gameObject;

        // prevent zombie friendly fire
        if (data.receiver.CompareTag("Zombie") && other.CompareTag("Zombie"))
        {
            return;
        }

        if (!(other.CompareTag("Zombie") || other.CompareTag("Survivor")))
        {
            return;
        }

        EventManager.RaiseOnClutch(data);

        // Debug.Log($"{data.receiver} is grabbed by {data.initiator}");
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other == null)
        {
            return;
        }

        data.receiver = other.gameObject;

        // prevent zombie friendly fire
        if (data.receiver.CompareTag("Zombie") && other.CompareTag("Zombie"))
        {
            return;
        }

        if (!(other.CompareTag("Zombie") || other.CompareTag("Survivor")))
        {
            return;
        }

        EventManager.RaiseOnRelease(data);

        // Debug.Log($"{data.receiver} is released by {data.initiator}");
    }
}
