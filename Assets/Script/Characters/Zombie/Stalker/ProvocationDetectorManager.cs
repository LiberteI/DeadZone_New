using System;
using UnityEngine;
[Serializable]
public class ProvocationData
{
    public GameObject receiver;

    public GameObject initiator;
}
public class ProvocationDetectorManager : MonoBehaviour
{
    public ProvocationData data;
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
        

        // Debug.Log($"{data.initiator} sees survivor : {data.receiver}");
        EventManager.RaiseOnProvoked(data);
    }
}
