using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turretHealth : MonoBehaviour
{
    [SerializeField] float health;

    public void TurretTakeDamage(float damagePoints)
    {
        health -= damagePoints;
        if (health <= 0f)
        {
            Destroy(gameObject);
        }
    }
    //Example how to use the method in an object that is supposed to damage the turret (P.S, was tested on OnCollisionEnter method)
    //private turretHealth th;
    //else if (other.gameObject.CompareTag("Enemy"))
    //{
    //    th = other.transform.root.gameObject.GetComponent<turretHealth>();
    //    if (th != null)
    //    {
    //        th.TurretTakeDamage(damage);
    //        Debug.Log("hit enemy");
    //        Destroy(gameObject);
    //    }
    //}

}
