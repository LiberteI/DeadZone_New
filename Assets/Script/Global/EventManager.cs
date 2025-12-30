using UnityEngine;
using System;
using Unity.VisualScripting;
public static class EventManager
{
    public static event Action<BulletHitData> OnBulletHit;

    public static event Action<MeleeHitData> OnMeleeHit;

    public static event Action<GameObject> OnSurvivorDied;

    public static event Action OnBaseBroken;
    public static event Action OnStopSpawning;

    public static event Action<GameObject> OnZombieDie;

    public static event Action OnClearLevel;

    public static event Action<ClutchData> OnClutch;

    public static event Action<ClutchData> OnRelease;

    public static event Action<ProvocationData> OnProvoked;

    public static event Action<GameObject> OnScream;

    public static event Action<GameObject> OnIsWithinScreamRange;

    public static event Action<DetectionData> OnSeeSurvivor;

    public static event Action<DetectionData> OnControlSuccessful;

    public static event Action<GameObject> OnLootCorpse;

    public static event Action<DetectionData> OnSeeZombie;

    public static event Action<GameObject> OnAlterAIType;

    public static event Action<GameObject, bool> OnFloorChanged;

    public static event Action OnMiniHorde;

    public static event Action OnSmallHorde;

    public static event Action OnGrandHorde;

    public static event Action<float> OnGunShot;

    public static event Action OnSendMessage;

    public static void RaiseSOS()
    {
        if (OnSendMessage != null)
        {
            OnSendMessage();
        }
        
    }

    public static void RaiseMiniHorde()
    {
        if (OnMiniHorde != null)
        {
            OnMiniHorde();
        }
    }

    public static void RaiseSmallHorde()
    {
        if (OnSmallHorde != null)
        {
            OnSmallHorde();
        }
    }

    public static void RaiseGrandHorde()
    {
        if (OnGrandHorde != null)
        {
            OnGrandHorde();
        }
    }
    public static void RaiseOnGunShot(float noise)
    {
        if (OnGunShot != null)
        {
            OnGunShot(noise);
        }
    }



    public static void RaiseOnFloorChanged(GameObject obj, bool shouldIncrement)
    {
        if (OnFloorChanged != null)
        {
            OnFloorChanged(obj, shouldIncrement);
        }
    }

    public static void RaiseOnAlterAIType(GameObject survivor)
    {
        if (OnAlterAIType != null)
        {
            OnAlterAIType(survivor);
        }
    }

    
    public static void RaiseOnSeeZombie(DetectionData data)
    {
        if (OnSeeZombie != null)
        {
            OnSeeZombie(data);
        }
    }

    public static void RaiseOnLootCorpose(GameObject zombie)
    {
        if (OnLootCorpse != null)
        {
            OnLootCorpse(zombie);
        }
    }

    public static void RaiseOnControlSuccessful(DetectionData data)
    {
        if (OnControlSuccessful != null)
        {
            OnControlSuccessful(data);
        }
    }

    public static void RaiseOnSeeSurvivor(DetectionData data)
    {
        if (OnSeeSurvivor != null)
        {
            OnSeeSurvivor(data);
        }
    }

    public static void RaiseOnIsWithinScreamRange(GameObject receiver)
    {
        if (OnIsWithinScreamRange != null)
        {
            OnIsWithinScreamRange(receiver);
        }
    }

    public static void RaiseOnScream(GameObject screamer)
    {
        if (OnScream != null)
        {
            OnScream(screamer);
        }
    }

    public static void RaiseOnProvoked(ProvocationData data)
    {
        if (OnProvoked != null)
        {
            OnProvoked(data);
        }
    }

    public static void RaiseOnRelease(ClutchData data)
    {
        if (OnRelease != null)
        {
            OnRelease(data);
        }
    }

    public static void RaiseOnClutch(ClutchData data)
    {
        if (OnClutch != null)
        {
            OnClutch(data);
        }
    }

    public static void RaiseOnClearLevel()
    {
        if (OnClearLevel != null)
        {
            OnClearLevel();
        }
    }

    public static void RaiseOnZombieDie(GameObject zombie)
    {
        if (OnZombieDie != null)
        {
            OnZombieDie(zombie);
        }
    }
    public static void RaiseStopSpawning()
    {
        if (OnStopSpawning != null)
        {
            OnStopSpawning();
        }
    }

    public static void RaiseOnBulletHit(BulletHitData data)
    {
        if (OnBulletHit != null)
        {
            OnBulletHit(data);
        }
    }

    public static void RaiseOnMeleeHit(MeleeHitData data)
    {
        if (OnMeleeHit != null)
        {
            OnMeleeHit(data);
        }
    }

    public static void RaiseOnSurvivorDied(GameObject survivor)
    {
        if (OnSurvivorDied != null)
        {
            OnSurvivorDied(survivor);
        }
    }

    public static void RaiseOnBaseBroken()
    {
        if (OnBaseBroken != null)
        {
            OnBaseBroken();
        }
    }
    
}
