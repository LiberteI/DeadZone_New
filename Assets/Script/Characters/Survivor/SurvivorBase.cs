using UnityEngine;


public class SurvivorParameters
{
    public Rigidbody2D RB;

    public Animator animator;

    public SurvivorAI aiManager;

    public bool isFacingRight;

    public GameObject survivorContainer;
}
public class SurvivorBase : MonoBehaviour
{
    protected SurvivorIState currentState;

    public SurvivorParameters parameter;

    public bool isPlayedByPlayer;

    public int curFloor = 1;
    void Update()
    {
        // Debug.Log(parameter);
        currentState.OnUpdate();

        if (!isPlayedByPlayer)
        {
            return;
        }
        currentState.HandleInput();
    }
    void OnEnable()
    {
        EventManager.OnFloorChanged += UpdateCurFloor;
        // Debug.Log($"Subscribed for {this.gameObject}");
    }

    void OnDisable()
    {
        EventManager.OnFloorChanged -= UpdateCurFloor;
        // Debug.Log($"UpSubscribed for {this.gameObject}");
    }
    protected void UpdateCurFloor(GameObject obj, bool shouldIncrement)
    {
        // Debug.Log("update cur floor get called");
        if (obj != this.gameObject)
        {
            // Debug.Log($"this survivor: {parameter.survivorContainer}, should be updated: {obj}");
            return;
        }

        if (shouldIncrement)
        {
            curFloor += 1;
        }
        else
        {
            curFloor -= 1;
        }
    }
}