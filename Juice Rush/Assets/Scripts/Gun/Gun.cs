using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private GunData gunData;
    [SerializeField] private GameObject Bullet;
    [SerializeField] private Transform FirePosition;
    [SerializeField] private Transform playerCamera;

    float timeSinceLastShot;

    private void Start()
    {
        PlayerShoot.shootInput += Shoot;
        PlayerShoot.reloadInput += StartReload;
    }
    public void StartReload()
    {
        if (!gunData.reloading)
        {
            StartCoroutine(Reload());        
        }
    }
    private IEnumerator Reload()
    {
        gunData.reloading = true;

        yield return new WaitForSeconds(gunData.reloadTime);

        gunData.currentAmmo = gunData.magSize;

        gunData.reloading = false;
    }
    private bool CanShoot() => !gunData.reloading && timeSinceLastShot > 1f / (gunData.fireRate / 60f);
    private void Shoot()
    {
        if (gunData.currentAmmo > 0)
        {
            if (CanShoot())
            {
                Instantiate(Bullet, FirePosition.transform.position, FirePosition.transform.rotation);
                gunData.currentAmmo--;
                timeSinceLastShot = 0;
                OnGunShot();
            }
        }
    }
    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;

        //makes the gun follow the camera's rotation 
        transform.rotation = playerCamera.transform.rotation;

        //visualisation of raycast for gun
        Debug.DrawRay(FirePosition.position, FirePosition.forward * 100);
    }

    private void OnGunShot()
    {
         
    }
}
