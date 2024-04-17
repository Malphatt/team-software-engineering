using UnityEngine;

public class weapon : MonoBehaviour
{
    public string WeaponType;

    public void OnAttack(string context)
    {
        switch (WeaponType)
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
                Debug.Log("Weapon not found");
                break;

        }
    }

    public void OnADS(string context)
    {
        if (context == "Started")
        {
            Debug.Log("ADS");
        }
        else if (context == "Canceled")
        {
            Debug.Log("Stopped ADS");
        }
    }

    public void OnReload(string context)
    {
        if (context == "Started")
        {
            Debug.Log("Reloading");
        }
        else if (context == "Canceled")
        {
            Debug.Log("Stopped Reloading");
        }
    }
}
