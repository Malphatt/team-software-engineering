using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunScript : MonoBehaviour
{
    public int CurrentAmmo { get; private set; }
    public bool Reloading { get; private set; }
    public bool ADS { get; private set; } 
    [SerializeField] Transform MuzzlePoint;
    [SerializeField] GameObject BulletPrefab;

    Transform StartPoint;
    Transform HitPoint;

    public void StartAttack(Transform targetLocation)
    {
        if (CurrentAmmo <= 0 && !Reloading) StartReload();
        if (Reloading) return;

        CurrentAmmo--;
        Debug.Log("Gun Fired");

        if (targetLocation != HitPoint)
        {
            StartPoint = new GameObject().transform;
            StartPoint.position = MuzzlePoint.position;
            HitPoint = targetLocation;

            // Instantiate Bullet facing the hit point
            GameObject bullet = Instantiate(BulletPrefab, MuzzlePoint.position, Quaternion.LookRotation(HitPoint.position - MuzzlePoint.position));
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 100f;
            Destroy(bullet, 1f);

            // Debug.Log("Gun Hit Enemy");
            // Animate?
        }
        else
        {
            // Instantiate Bullet facing forward
            GameObject bullet = Instantiate(BulletPrefab, MuzzlePoint.position, MuzzlePoint.rotation);
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 100f;
            Destroy(bullet, 1f);
        }
    }

    public void StopAttack()
    {
        // Do Nothing
    }

    public void StartADS()
    {
        // Animate?
    }

    public void StopADS()
    {
        // Animate?
    }

    public void StartReload()
    {
        if (!Reloading) StartCoroutine(Reload());
    }

    public void StopReload()
    {
        // Do Nothing
    }

    void Start()
    {
        CurrentAmmo = gameObject.GetComponent<Weapon>().WeaponData.MagazineSize;
    }

    void Update()
    {
        if (StartPoint != null && HitPoint != null)
            Debug.DrawRay(StartPoint.position, HitPoint.position - StartPoint.position);
    }

    private IEnumerator Reload()
    {
        Reloading = true;

        yield return new WaitForSeconds(gameObject.GetComponent<Weapon>().WeaponData.ReloadTime);

        Reloading = false;
        CurrentAmmo = gameObject.GetComponent<Weapon>().WeaponData.MagazineSize;
    }
}
