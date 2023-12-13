using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class weaponController : MonoBehaviour
{
    [SerializeField] GameObject weaponContainer;
    private GameObject[] weaponsArray;

    void Awake()
    {
        // Get all weapons in the weapon container and add them to the weapons array
        weaponsArray = new GameObject[weaponContainer.transform.childCount];
        for (int i = 0; i < weaponContainer.transform.childCount; i++)
        {
            weaponsArray[i] = weaponContainer.transform.GetChild(i).gameObject;
        }
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
            Debug.Log("Attack");
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            Debug.Log("Stopped Attacking");
        }
    }

//TODO: Implement
    public void OnADS(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Debug.Log("ADS");
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            Debug.Log("Stopped ADS");
        }
    }

//TODO: Implement
    public void OnReload(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Debug.Log("Reload");
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            Debug.Log("Stopped Reloading");
        }
    }

    //TODO: Implement
    public void OnSwapWeapon(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Debug.Log("Swap Weapon");
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            Debug.Log("Stopped Swapping Weapon");
        }
    }

//TODO: Implement
    public void OnPrimaryWeapon(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Debug.Log("Primary Weapon");
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            Debug.Log("Stopped Primary Weapon");
        }
    }

//TODO: Implement
    public void OnSecondaryWeapon(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Debug.Log("Secondary Weapon");
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            Debug.Log("Stopped Secondary Weapon");
        }
    }
}
