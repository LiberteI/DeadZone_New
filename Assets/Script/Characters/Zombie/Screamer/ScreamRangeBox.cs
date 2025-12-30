using UnityEngine;
public class ScreamRangeBox : MonoBehaviour
{
    [SerializeField] private GameObject receiver;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other == null)
        {
            return;
        }
        if (!other.CompareTag("Zombie"))
        {
            return;
        }
        receiver = other.gameObject;
        
        EventManager.RaiseOnIsWithinScreamRange(receiver);
    }
}
