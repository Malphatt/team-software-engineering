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
    [SerializeField] Animator anim;
    [SerializeField] GameObject gun;
    public ParticleSystem muzzleFlash;
    private float timer;
    Transform StartPoint;
    Transform HitPoint;
    private void OnEnable()
    {
        timer = 0;

        if (Reloading) Reloading = false;
        // If the gun has no ammo, reload
        if (CurrentAmmo <= 0 && !Reloading) StartReload();
    }
    public void StartAttack(Transform targetLocation)
    {
        if (CurrentAmmo <= 0 && !Reloading) StartReload();
        if (Reloading) return;
        anim.SetBool("isShooting", true);
        timer = 0;
        CurrentAmmo--;
        muzzleFlash.Play();

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

    void Awake()
    {
        CurrentAmmo = gameObject.GetComponent<Weapon>().WeaponData.MagazineSize;
        gun = this.gameObject;
        anim = gun.GetComponent<Animator>();
    }

    void Update()
    {
        if (StartPoint != null && HitPoint != null)
            Debug.DrawRay(StartPoint.position, HitPoint.position - StartPoint.position);
        timer += Time.deltaTime;
        anim.SetFloat("timePassed", timer);
        if (timer > 0.05f)
            anim.SetBool("isShooting", false);
    }

    private IEnumerator Reload()
    {
        anim.SetBool("isReloading", true);
        Reloading = true;
        yield return new WaitForSeconds(5);
        anim.SetBool("isReloading", false);
        Reloading = false;
        CurrentAmmo = gameObject.GetComponent<Weapon>().WeaponData.MagazineSize;
    }
}
