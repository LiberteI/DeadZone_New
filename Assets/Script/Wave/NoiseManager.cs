using System.Security.Cryptography;
using Unity.Mathematics;
using UnityEngine;

public class NoiseManager : MonoBehaviour
{
    /*
        Zombie types: runner, clutcher, stalker, poisoner, screamer, boomer, tank, jockey

        Use a noise meter for controlling zombie generation.

            *1. When the noise meter is less than 30%: randomly generate a few zombies every few seconds.

            *2. When the noise meter is above 30%: trigger mini wave. A crowd of zombies are generated at once within a random range.

            *3. When the noise meter is above 60%: trigger a horde.

            *4. When the noise meter is above 90%: trigger a big horde.

            (themed horde is controlled by radio station only)
            5. if the noise meter is maxed: Trigger a grand horde with special theme: choosing randomly from what is contained below (Naturally impossible)
                1). Swarm of Runners (Blitzkrieg)

                    Massive wave of fast, low-HP runners.

                    Overwhelms with speed, not durability.

                    Creates panic → forces player to reposition.
                2). Many poisoners + boomers.

                    AoE clouds stack to restrict areas of the map.

                    Survivors are forced to move out of their camp zone.
                3). infantry-tank synergy: generate some tanks together with other zombies.
                4). Multiple screamers that constantly aggro more zombies.

                    Waves chain in real-time until screamers are killed.

                    Feels chaotic and endless until the player prioritizes screamers.
                5). Mutation Storm

                    Random mix of all special mutants (jockey, poisoner, boomer, tank).

                    Feels unpredictable and overwhelming.

                    Rare “boss-style” grand horde for late game

        Few things would increase the noise meter:
            1. reckless shooting: shooting adds noise. about 0.1% per shoot

            2. grenade explosion: explosion adds noise about 5% per explosion

            3. radio station trigger maximise noise instantly, serving the thread

            4. Screamer increases noise about 5% per scream

            
            To avoid infinite escalation:

                1. start a timer after grand horde, when no more waves will be triggered

        Noise Decay:
            1. decay fast below 30%
            2. decay moderately at 30% - 60%
            3. decay slowly above 60%
            4. freeze decaying until resolved if grand horde is triggered
            5. Decay (natural drop over time) → should never trigger waves.

                Deduction (wave resolution) → should also never trigger waves.

                Only player-driven increases should cause threshold checks.
    */
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
                EventManager.RaiseGrandHorde();
                return;
            }
            
        }

        if (noiseMeter >= SMALL_HORDE_THRESHOLD)
        {
            if (oriValue < SMALL_HORDE_THRESHOLD)
            {
                // raise small horde
                EventManager.RaiseSmallHorde();
                return;
            }
            
        }

        if (noiseMeter >= MINI_WAVE_THRESHOLD)
        {
            if (oriValue < MINI_WAVE_THRESHOLD)
            {
                // raise mini wave 
                EventManager.RaiseMiniHorde();
                return;
            }
            
        }
    }
}
