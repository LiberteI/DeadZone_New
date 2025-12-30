using UnityEngine;

public class SWATShootingManager : BaseShootingManager
{
    private SWAT swat;

    private SWATParameter swatParameter;

    void Start()
    {
        swat = (SWAT)base.survivor;

        swatParameter = swat.parameter;
    }

}
