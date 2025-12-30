using UnityEngine;

public class BaseSurvivorHealthManager : MonoBehaviour
{
    /*
        This script is responsible for keeping track of survivor's health status.

        The health will get deducted if damage is taken.

        If health drops to and below 0, survivor dies instantly.
    */

    [SerializeField] private float maxHealth;

    [SerializeField] private float curHealth;

    [SerializeField] private bool isDead;

    void OnEnable(){
        EventManager.OnMeleeHit += TakeDamage;
    }
    void OnDisable(){
        EventManager.OnMeleeHit -= TakeDamage;
    }

    void Start(){
        curHealth = maxHealth;
    }

    void Update(){
        TryDying();
    }
    public void TakeDamage(MeleeHitData data){
        // receiver filter
        if(this.gameObject != data.receiver){
            return;
        }
        curHealth -= data.damage;
    }

    private void TryDying(){
        if(isDead){
            return;
        }
        if (curHealth <= 0)
        {
            isDead = true;

            EventManager.RaiseOnSurvivorDied(this.gameObject);
        }
    }
}
