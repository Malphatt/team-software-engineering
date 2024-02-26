using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyRotation : MonoBehaviour
{
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
}
