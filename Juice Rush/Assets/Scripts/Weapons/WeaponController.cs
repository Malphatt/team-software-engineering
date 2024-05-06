using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponController : MonoBehaviour
{
    [SerializeField] GameObject PlayerCamera;
    MeleeHitbox MeleeHitbox;

    [SerializeField] GameObject WeaponContainer;
    private GameObject[] weaponsArray;
    private int weaponIndex;

    bool canAttack = true;

    Transform targetLocation;

    void Awake()
    {
        // Get all weapons in the weapon container and add them to the weapons array
        weaponsArray = new GameObject[WeaponContainer.transform.childCount];
        for (int i = 0; i < WeaponContainer.transform.childCount; i++)
        {
            weaponsArray[i] = WeaponContainer.transform.GetChild(i).gameObject;
        }
        weaponIndex = 0;

        MeleeHitbox = GetComponent<MeleeHitbox>();
        targetLocation = null;
    }

    void DealDamage()
    {
        if (weaponsArray[weaponIndex].GetComponent<Weapon>().WeaponData.Class == WeaponData.Classes.Melee)
        {
            Colliders enemies = MeleeHitbox.GetColliders(weaponsArray[weaponIndex].GetComponent<Weapon>().WeaponData.Range);
            GameObject[] headColliders = enemies.HeadColliders;
            List<GameObject> bodyCollidersList = new List<GameObject>(enemies.BodyColliders);

            // Filter out the enemies from the body colliders that are also in the head colliders
            foreach (GameObject headCollider in headColliders)
            {
                bodyCollidersList.Remove(headCollider);
            }

            GameObject[] bodyColliders = bodyCollidersList.ToArray();

            try
            {
                foreach (GameObject headCollider in headColliders)
                    headCollider.GetComponent<Enemy>().TakeDamage(weaponsArray[weaponIndex].GetComponent<Weapon>().WeaponData.Damage * weaponsArray[weaponIndex].GetComponent<Weapon>().WeaponData.HeadshotMultiplier);

                foreach (GameObject bodyCollider in bodyColliders)
                    bodyCollider.GetComponent<Enemy>().TakeDamage(weaponsArray[weaponIndex].GetComponent<Weapon>().WeaponData.Damage);
            }
            catch (System.NullReferenceException)
            {
                Debug.Log("No enemies in range.");
            }

        }
        else if (weaponsArray[weaponIndex].GetComponent<Weapon>().WeaponData.Class == WeaponData.Classes.Ranged)
        {
            RaycastHit[] hits;
            hits = Physics.RaycastAll(PlayerCamera.transform.position, PlayerCamera.transform.forward, weaponsArray[weaponIndex].GetComponent<Weapon>().WeaponData.Range);

            if (hits.Length > 0)
            {
                // Reverse the hits array so that the closest hit is the first element
                System.Array.Reverse(hits);

                int enemiesHit = 0;
                for (int i = 0; i < hits.Length; i++)
                {
                    // If the hit object is a wall then break the loop (no damage dealt through walls)
                    if (hits[i].collider.gameObject.CompareTag("Wall"))
                        break;

                    // If the hit object is an enemy, deal damage to it
                    if (hits[i].collider.gameObject.CompareTag("Enemy"))
                    {
                        // If the enemy is behind another enemy, reduce the damage dealt
                        float wallbangMultiplier = 1 / (enemiesHit + 1);

                        // If the hit distance is greater than the drop off range, reduce the damage dealt
                        float dropOffMultiplier = 1;
                        if (hits[i].distance > weaponsArray[weaponIndex].GetComponent<Weapon>().WeaponData.DropOffRange && weaponsArray[weaponIndex].GetComponent<Weapon>().WeaponData.DropOffRange != 0)
                            dropOffMultiplier = weaponsArray[weaponIndex].GetComponent<Weapon>().WeaponData.DropOffMultiplier;

                        // Set the target location to the hit point
                        targetLocation = new GameObject().transform;
                        targetLocation.position = hits[i].point;

                        // Deal damage to the enemy
                        hits[i].collider.gameObject.GetComponent<Enemy>().TakeDamage(weaponsArray[weaponIndex].GetComponent<Weapon>().WeaponData.Damage * wallbangMultiplier * dropOffMultiplier);
                        enemiesHit++;
                    }
                }
            }

            // Clear the hits array
            hits = null;
        }
    }

    void Update()
    {
        // Debug
        Debug.DrawRay(PlayerCamera.transform.position, PlayerCamera.transform.forward * weaponsArray[weaponIndex].GetComponent<Weapon>().WeaponData.Range, Color.yellow);
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && canAttack)
        {
            DealDamage();
            weaponsArray[weaponIndex].GetComponent<Weapon>().OnAttack("Started", targetLocation);
            StartCoroutine(AttackCooldown());
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

    IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(weaponsArray[weaponIndex].GetComponent<Weapon>().WeaponData.AttackCooldown);
        canAttack = true;
    }
}
