using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponData WeaponData;

    public void OnAttack(string context)
    {
        switch (WeaponData.Name)
        {
            case "Katana":
                if (context == "Started") gameObject.GetComponent<katanaScript>().StartAttack();
                else if (context == "Canceled") gameObject.GetComponent<katanaScript>().StopAttack();
                break;
            case "Gun":
                //if (context == "Started") gameObject.GetComponent<gunScript>().StartAttack();
                //else if (context == "Canceled") gameObject.GetComponent<gunScript>().StopAttack();
                break;
            default:
                Debug.Log("I don't know what to do with the weapon: " + WeaponData.Name);
                break;

        }
    }

    public void OnADS(string context)
    {
        switch (WeaponData.Name)
        {
            case "Katana":
                // Do Nothing
                break;
            case "Gun":
                //if (context == "Started") gameObject.GetComponent<gunScript>().StartADS();
                //else if (context == "Canceled") gameObject.GetComponent<gunScript>().StopADS();
                break;
            default:
                Debug.Log("I don't know what to do with the weapon: " + WeaponData.Name);
                break;

        }
    }

    public void OnReload(string context)
    {
        switch (WeaponData.Name)
        {
            case "Katana":
                // Do Nothing
                break;
            case "Gun":
                //if (context == "Started") gameObject.GetComponent<gunScript>().StartReload();
                //else if (context == "Canceled") gameObject.GetComponent<gunScript>().StopReload();
                break;
            default:
                Debug.Log("I don't know what to do with the weapon: " + WeaponData.Name);
                break;

        }
    }
}
