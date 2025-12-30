using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

/*
    usage: set initiator in Inspector
    
    get the rest of 3 during runtime:

    receiver: OnTriggerEnter2D

    damage: set damage in melee manager

    hitIncomingDir : initiator's facing dir
*/
[Serializable]
public class MeleeHitData{
    // who initiates the hit
    public GameObject initiator;

    // who receive the hit
    public GameObject receiver;

    // initiator's damage
    public float damage;    

    // initiator's hit dir
    public Vector3 hitIncomingDir;
}
/*
    This script is responsible for fetching hit data
*/
public class MeleeHitBoxManager : MonoBehaviour
{
    public MeleeHitData data;

    public void SetData(float damage, Vector3 hitIncomingDir){
        data.damage = damage;

        data.hitIncomingDir = hitIncomingDir;
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other == null){
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

        
        // to prevent self hit: set no interation in project setting

        EventManager.RaiseOnMeleeHit(data);

        // Debug.Log($"{data.initiator} hits {data.receiver} with damage {data.damage} facing {data.hitIncomingDir}");

    }
}
