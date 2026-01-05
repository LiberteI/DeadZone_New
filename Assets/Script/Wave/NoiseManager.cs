using System.Security.Cryptography;
using Unity.Mathematics;
using UnityEngine;

public class NoiseManager : MonoBehaviour
{
    
    [Header("NOISE THRESHOLD")]
    private const float MINI_WAVE_THRESHOLD = 30f;

    private const float SMALL_HORDE_THRESHOLD = 60f;

    private const float GRAND_HORDE_THRESHOLD = 90f;

    [Header("CURRENT NOISE")]
    [SerializeField] private float noiseMeter;

    // updated everytime meter rises
    private float decayMeterPauseTimer = 0.5f;

    private float decayMultiplyer;

    void OnEnable()
    {
        EventManager.OnGunShot += AddNoise;
    }
    void OnDisable()
    {
        EventManager.OnGunShot -= AddNoise;
    }
    void Update()
    {
        TryUpdateDecayPauseTimer();

        TryDecayMeter();
        
    }

    private void TryUpdateDecayPauseTimer()
    {
        if (decayMeterPauseTimer > 0)
        {
            decayMeterPauseTimer -= Time.deltaTime;
        }
    }

    private void TryDecayMeter()
    {
        decayMultiplyer = 1f;
        if (decayMeterPauseTimer > 0)
        {
            return;
        }

        if (noiseMeter <= 0)
        {
            return;
        }

        if (noiseMeter <= MINI_WAVE_THRESHOLD)
        {
            decayMultiplyer = 10f;
        }
        else if (noiseMeter > MINI_WAVE_THRESHOLD && noiseMeter < SMALL_HORDE_THRESHOLD)
        {
            decayMultiplyer = 7f;
        }
        else
        {
            decayMultiplyer = 5f;
        }
        noiseMeter -= Time.deltaTime * decayMultiplyer;
    }

    // updated by event
    private void AddNoise(float value)
    {
        if (noiseMeter >= 100)
        {
            return;
        }

        noiseMeter += value;

        decayMeterPauseTimer = 0.5f;

        // raise waves only in an ascending way
        TryRaiseWaves(noiseMeter - value);
    }

    private void TryRaiseWaves(float oriValue)
    {
        if (noiseMeter >= GRAND_HORDE_THRESHOLD)
        {
            if (oriValue < GRAND_HORDE_THRESHOLD)
            {
                // raise grand horde
                HordeEvents.RaiseGrandHorde();
                return;
            }
            
        }

        if (noiseMeter >= SMALL_HORDE_THRESHOLD)
        {
            if (oriValue < SMALL_HORDE_THRESHOLD)
            {
                // raise small horde
                HordeEvents.RaiseSmallHorde();
                return;
            }
            
        }

        if (noiseMeter >= MINI_WAVE_THRESHOLD)
        {
            if (oriValue < MINI_WAVE_THRESHOLD)
            {
                // raise mini wave 
                HordeEvents.RaiseMiniHorde();
                return;
            }
            
        }
    }
}
