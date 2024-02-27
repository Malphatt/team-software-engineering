using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testingPlayerHealth : MonoBehaviour
{
    [SerializeField] float health = 100f;

    public void TakeDamage(float damagePoints)
    {
        health -= damagePoints;
        if (health <= 0f)
        {
            Debug.Log("hit player");
            Destroy(gameObject);
        }
    }
}
