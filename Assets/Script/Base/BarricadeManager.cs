using UnityEngine;

public class BarricadeManager : MonoBehaviour
{
    void OnEnable()
    {
        EventManager.OnSurvivorDied += TryDestroyBarrier;
    }
    void OnDisable()
    {
        EventManager.OnSurvivorDied -= TryDestroyBarrier;
    }

    private void TryDestroyBarrier(GameObject targetObj)
    {
        if (this.gameObject != targetObj)
        {
            return;
        }

        Destroy(this.gameObject);
    }
}
