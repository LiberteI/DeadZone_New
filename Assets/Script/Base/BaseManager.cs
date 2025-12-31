using UnityEngine;

public class BaseManager : MonoBehaviour
{

    public static BaseManager Instance
    {
        get; private set;
    }
    public float maxHealth;

    public float curHealth;

    public bool isBroken;

    public GameObject baseObj;

    public Transform level1Door;

    public Transform level2Door;

    [SerializeField]
    private float radioMessageTimer;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    void OnEnable()
    {
        EventManager.OnMeleeHit += TakeDamage;
    }
    void OnDisable()
    {
        EventManager.OnMeleeHit -= TakeDamage;
    }
    void Start()
    {
        curHealth = maxHealth;
    }
    void Update()
    {
        TryBreaking();

        TryUpdateSOSMessageCoolDown();
    }

    public void TakeDamage(MeleeHitData data)
    {
        // receiver filter
        if (baseObj != data.receiver)
        {
            return;
        }
        curHealth -= data.damage;
    }

    private void TryBreaking()
    {
        if (isBroken)
        {
            return;
        }

        if (curHealth <= 0)
        {
            EventManager.RaiseOnBaseBroken();
            Debug.Log("YOU LOSE");
            isBroken = true;
        }
    }

    public void SendSOSMessage()
    {
        if (radioMessageTimer > 0)
        {
            return;
        }

        Debug.Log("SOS Sent");
        EventManager.RaiseSOS();

        radioMessageTimer = 60f;
    }

    private void TryUpdateSOSMessageCoolDown()
    {
        if (radioMessageTimer >= 0)
        {
            radioMessageTimer -= Time.deltaTime;
        }
    }
}
