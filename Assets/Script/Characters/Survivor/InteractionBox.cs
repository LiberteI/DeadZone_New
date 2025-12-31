using UnityEngine;

public class InteractionBox : MonoBehaviour
{
    [SerializeField] private GameObject playerToTeleport;
    private void OnTriggerStay2D(Collider2D other)
    {

        if (other == null)
        {
            return;
        }
        if (other.CompareTag("Radio"))
        {
            // Debug.Log("entered radio");
            if (Input.GetKey("e"))
            {
                // Debug.Log("Sent");
                BaseManager.Instance.SendSOSMessage();
            }
        }
        // Debug.Log($"Entered a collider {other}");
        if (other.CompareTag("Door"))
        {
            // Debug.Log("fired");
            if (Input.GetKey("w"))
            {
                TeleportManager door = other.GetComponent<TeleportManager>();

                door.Teleport(playerToTeleport);
            }
        }
        if (other.CompareTag("Corpse"))
        {
            // Debug.Log($"Entered a Corpse {other.gameObject}");
            if (Input.GetKey("e"))
            {
                // Debug.Log("Looted a corpse");
                EventManager.RaiseOnLootCorpose(other.gameObject);
                return;
            }

        }
        if (other.CompareTag("Survivor"))
        {
            if (Input.GetKey("e"))
            {
                // Debug.Log("Looted a corpse");
                EventManager.RaiseOnAlterAIType(other.gameObject);
                return;
            }

        }
    }
}
