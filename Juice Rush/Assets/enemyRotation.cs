using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyRotation : MonoBehaviour
{
    [SerializeField] float health;
    [SerializeField] float aimSpeed;
    public void shotgunEnemyRotation(Transform player)
    {
        Vector3 direction = (player.position - transform.position);
        Debug.Log(direction.x);
        //Determines the rotation towards the player
        Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);
        Debug.Log(lookRotation);
        //Rotates towards the player using Lerp for a smoother transition
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * aimSpeed);
    }
    public void ShotgunEnemyTakeDamage(float damagePoints)
    {
        health -= damagePoints;
        if (health < 0)
        {
            //Destroy the parent object
            Destroy(transform.parent.gameObject);

        }
    }
    //Example how to use the method in an object that is supposed to damage the shotgun enemy(P.S, was tested on OnCollisionEnter method)
    //private enemyRotation ec;
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if(other.gameObject.CompareTag("Enemy"))
    //    {
    //         ec = other.gameObject.GetComponent<enemyRotation>();
    //        if (ec != null)
    //        {
    //            ec.ShotgunEnemyTakeDamage(damage);
    //            Debug.Log("hit enemy");
    //            Destroy(gameObject);
    //        }
    //    }
    //}


}
