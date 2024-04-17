using UnityEngine;

public class weapon : MonoBehaviour
{
    public string WeaponName;
    public enum WeaponTypes { Primary, Secondary }
    public WeaponTypes WeaponType;

    public void OnAttack(string context)
    {
        switch (WeaponName)
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
        switch (WeaponName)
        {
            case "Katana":
                // Do Nothing
                break;
            case "Gun":
                //if (context == "Started") gameObject.GetComponent<gunScript>().StartADS();
                //else if (context == "Canceled") gameObject.GetComponent<gunScript>().StopADS();
                break;
            default:
                Debug.Log("Weapon not found");
                break;

        }
    }

    public void OnReload(string context)
    {
        switch (WeaponName)
        {
            case "Katana":
                // Do Nothing
                break;
            case "Gun":
                //if (context == "Started") gameObject.GetComponent<gunScript>().StartReload();
                //else if (context == "Canceled") gameObject.GetComponent<gunScript>().StopReload();
                break;
            default:
                Debug.Log("Weapon not found");
                break;

        }
    }
}
