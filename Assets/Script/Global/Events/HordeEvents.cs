using UnityEngine;
using System;
public static class HordeEvents
{
    
    public static event Action OnMiniHorde;

    public static event Action OnSmallHorde;

    public static event Action OnGrandHorde;

    public static event Action OnSendMessage;

    public static event Action OnHordeStart;

    public static event Action OnHordeEnd;

    public static void RaiseHordeStart()
    {
        if (OnHordeStart != null)
        {
            OnHordeStart();
        }
    }

    public static void RaiseHordeEnd()
    {
        if(OnHordeEnd != null)
        {
            OnHordeEnd();
        }
    }
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
}