using UnityEngine;
using UnityEngine.InputSystem;

public class weaponController : MonoBehaviour
{
    [SerializeField] GameObject weaponContainer;
    private GameObject[] weaponsArray;
    private int weaponIndex;

    // Weapon states
    public enum WeaponState
    {
        primary,
        secondary,
        reloading,
        swapping
    }

    void Awake()
    {
        // Get all weapons in the weapon container and add them to the weapons array
        weaponsArray = new GameObject[weaponContainer.transform.childCount];
        for (int i = 0; i < weaponContainer.transform.childCount; i++)
        {
            weaponsArray[i] = weaponContainer.transform.GetChild(i).gameObject;
        }
        weaponIndex = 0;
    }

    void Update()
    {

    }

    void FixedUpdate()
    {

    }

    //TODO: Implement
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            weaponsArray[weaponIndex].GetComponent<weapon>().OnAttack("Started");
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            weaponsArray[weaponIndex].GetComponent<weapon>().OnAttack("Canceled");
        }
    }

    //TODO: Implement
    public void OnADS(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            weaponsArray[weaponIndex].GetComponent<weapon>().OnADS("Started");
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            weaponsArray[weaponIndex].GetComponent<weapon>().OnADS("Canceled");
        }
    }

    //TODO: Implement
    public void OnReload(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            weaponsArray[weaponIndex].GetComponent<weapon>().OnReload("Started");
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            weaponsArray[weaponIndex].GetComponent<weapon>().OnReload("Canceled");
        }
    }

    //TODO: Implement
    public void OnSwapWeapon(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Debug.Log("Swap Weapon");
        }
    }

    //TODO: Implement
    public void OnPrimaryWeapon(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {

        }
    }

    //TODO: Implement
    public void OnSecondaryWeapon(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Debug.Log("Secondary Weapon");
        }
    }
}
