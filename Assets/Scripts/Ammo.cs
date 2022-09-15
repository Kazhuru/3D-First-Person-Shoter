using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Ammo
{
    public int startingAmmo = 20;
    public int currentAmmo;
    public AmmoType ammoType;

    public void Init()
    {
        currentAmmo = startingAmmo;
    }

    public void ReduceAmmoAmount()
    {
        if(currentAmmo > 0)
            currentAmmo -= 1;
    }

    public void IncreaseAmmoAmount(int increaseValue)
    {
        currentAmmo += increaseValue;
    }

    public bool IsEmpty()
    {
        return currentAmmo == 0;
    }
}
