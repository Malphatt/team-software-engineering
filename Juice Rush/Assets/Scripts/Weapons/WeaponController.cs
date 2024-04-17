using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponController : MonoBehaviour
{
    [SerializeField] GameObject WeaponContainer;
    private GameObject[] weaponsArray;
    private int weaponIndex;

    void Awake()
    {
        // Get all weapons in the weapon container and add them to the weapons array
        weaponsArray = new GameObject[WeaponContainer.transform.childCount];
        for (int i = 0; i < WeaponContainer.transform.childCount; i++)
        {
            weaponsArray[i] = WeaponContainer.transform.GetChild(i).gameObject;
        }
        weaponIndex = 0;
    }

    void Update()
    {

    }

    void FixedUpdate()
    {

    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            weaponsArray[weaponIndex].GetComponent<Weapon>().OnAttack("Started");
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            weaponsArray[weaponIndex].GetComponent<Weapon>().OnAttack("Canceled");
        }
    }

    public void OnADS(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            weaponsArray[weaponIndex].GetComponent<Weapon>().OnADS("Started");
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            weaponsArray[weaponIndex].GetComponent<Weapon>().OnADS("Canceled");
        }
    }

    public void OnReload(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            weaponsArray[weaponIndex].GetComponent<Weapon>().OnReload("Started");
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            weaponsArray[weaponIndex].GetComponent<Weapon>().OnReload("Canceled");
        }
    }

    public void OnSwapWeapon(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (context.ReadValue<float>() > 0)
            {
                // Hide the current weapon
                weaponsArray[weaponIndex].SetActive(false);
                // Set the new weapon index
                weaponIndex++;
                if (weaponIndex >= weaponsArray.Length) weaponIndex = 0;
                // Show the new weapon
                weaponsArray[weaponIndex].SetActive(true);
            }
            else
            {
                // Hide the current weapon
                weaponsArray[weaponIndex].SetActive(false);
                // Set the new weapon index
                weaponIndex--;
                if (weaponIndex < 0) weaponIndex = weaponsArray.Length - 1;
                // Show the new weapon
                weaponsArray[weaponIndex].SetActive(true);
            }
        }
    }

    public void OnPrimaryWeapon(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            // Find the primary weapon in the weapons array
            for (int i = 0; i < weaponsArray.Length; i++)
            {
                if (weaponsArray[i].GetComponent<Weapon>().WeaponData.Type == WeaponData.WeaponTypes.Primary)
                {
                    if (weaponIndex == i) return;
                    // Hide the current weapon
                    weaponsArray[weaponIndex].SetActive(false);
                    // Set the new weapon index
                    weaponIndex = i;
                    // Show the new weapon
                    weaponsArray[weaponIndex].SetActive(true);
                    break;
                }
            }
        }
    }

    public void OnSecondaryWeapon(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            // Find the secondary weapon in the weapons array
            for (int i = 0; i < weaponsArray.Length; i++)
            {
                if (weaponsArray[i].GetComponent<Weapon>().WeaponData.Type == WeaponData.WeaponTypes.Secondary)
                {
                    if (weaponIndex == i) return;
                    // Hide the current weapon
                    weaponsArray[weaponIndex].SetActive(false);
                    // Set the new weapon index
                    weaponIndex = i;
                    // Show the new weapon
                    weaponsArray[weaponIndex].SetActive(true);
                    break;
                }
            }
        }
    }
}
