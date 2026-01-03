using UnityEngine;


public class ControlBoxManager : MonoBehaviour
{
    public DetectionData data;

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

        EventManager.RaiseOnControlSuccessful(data);

        // Debug.Log($"Captured {data.receiver}");
    }
}
