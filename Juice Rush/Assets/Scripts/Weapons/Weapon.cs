using System;
using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponData WeaponData;

    public void OnAttack(string context, Transform targetLocation = null)
    {
        switch (WeaponData.Name)
        {
            case WeaponData.WeaponNames.Katana:
                if (context == "Started") GetComponent<katanaScript>().StartAttack();
                else if (context == "Canceled") GetComponent<katanaScript>().StopAttack();
                break;
            case WeaponData.WeaponNames.Pistol:
                if (context == "Started") GetComponent<gunScript>().StartAttack(targetLocation);
                else if (context == "Canceled") GetComponent<gunScript>().StopAttack();
                break;
            default:
                Debug.Log("I don't know what to do with the weapon: " + WeaponData.Name.ToString());
                break;

        }
    }

    public void OnADS(string context)
    {
        switch (WeaponData.Name)
        {
            case WeaponData.WeaponNames.Katana:
                // Do Nothing
                break;
            case WeaponData.WeaponNames.Pistol:
                if (context == "Started") GetComponent<gunScript>().StartADS();
                else if (context == "Canceled") GetComponent<gunScript>().StopADS();
                break;
            default:
                Debug.Log("I don't know what to do with the weapon: " + WeaponData.Name.ToString());
                break;

        }
    }

    public void OnReload(string context)
    {
        switch (WeaponData.Name)
        {
            case WeaponData.WeaponNames.Katana:
                // Do Nothing
                break;
            case WeaponData.WeaponNames.Pistol:
                if (context == "Started") GetComponent<gunScript>().StartReload();
                else if (context == "Canceled") GetComponent<gunScript>().StopReload();
                break;
            default:
                Debug.Log("I don't know what to do with the weapon: " + WeaponData.Name.ToString());
                break;

        }
    }
}
