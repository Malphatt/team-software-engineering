using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyShotgun : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform gunMuzzle;
    [SerializeField] int bulletCount;
    [SerializeField] float shotSpread;
    [SerializeField] float shotForce;
    [SerializeField] float fireRate;
    [SerializeField] Transform player;
    float fireTime = 1f;

    public void FireShotgun()
    {
        if (Time.time > fireTime)
        {
            //Loops through the amount of bullets to be fired
            for (int i = 0; i < bulletCount; i++)
            {

                GameObject bullet = Instantiate(bulletPrefab, gunMuzzle.position, gunMuzzle.rotation);
                Rigidbody rb = bullet.GetComponent<Rigidbody>();

                //Generate random angle for spread on X/Y axis
                float spreadAngleX = Random.Range(-shotSpread, shotSpread);
                float spreadAngleY = Random.Range(-shotSpread, shotSpread);
                //Spread direction vector applied with the random spread angles to the gun muzzle
                Vector3 spreadShotDirection = Quaternion.Euler(spreadAngleX, spreadAngleY, 0) * gunMuzzle.forward;

                rb.AddForce(spreadShotDirection * shotForce, ForceMode.Impulse);

            }
            fireTime = Time.time + fireRate;
        }
    }
}
