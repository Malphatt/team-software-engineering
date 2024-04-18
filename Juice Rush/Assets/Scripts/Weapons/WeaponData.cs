using UnityEngine;

[CreateAssetMenu(fileName = "Weapon_Name", menuName = "WeaponData")]
public class WeaponData : ScriptableObject
{
    public enum WeaponTypes { Primary, Secondary };
    public enum Classes { Melee, Ranged };

    [Header("Weapon Info")]
    public string Name;
    public WeaponTypes Type;
    public Classes Class;
    public float Range; // How far the weapon can shoot or range of melee weapon

    [Header("Damage Values")]
    public float Damage;
    public float HeadshotMultiplier = 1.0f;


    [Header("Gun Values")]
    public float FireRate;
    public float ReloadTime;
    public int MagazineSize;
    public int MaxAmmo;
    public float Recoil;
    public float AdsSpeed;
    public float AdsFOV;
    public float AdsRecoil;
}