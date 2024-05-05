using UnityEngine;

[CreateAssetMenu(fileName = "Weapon_Name", menuName = "WeaponData")]
public class WeaponData : ScriptableObject
{
    public enum WeaponNames { Katana, Pistol };
    public enum WeaponTypes { Primary, Secondary };
    public enum Classes { Melee, Ranged };

    [Header("Weapon Info")]
    public WeaponNames Name;
    public WeaponTypes Type;
    public Classes Class;
    public float Range; // How far the weapon can shoot or range of melee weapon
    public float AttackCooldown; // Time between shots or attacks

    [Header("Damage Values")]
    public float Damage;
    public float HeadshotMultiplier = 1.0f;


    [Header("Gun Values")]
    public float ReloadTime;
    public int MagazineSize;
}