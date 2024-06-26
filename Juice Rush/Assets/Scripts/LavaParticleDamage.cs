using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaParticleDamage : MonoBehaviour
{
    [SerializeField] float damage;
    private testingPlayerHealth tps;
    float lifeTime = 4f;
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Apply damage to player (current player controller has no health?)
            tps = other.gameObject.GetComponent<testingPlayerHealth>();
            if (tps != null)
            {
                tps.TakeDamage(damage);
                Debug.Log("Destroyed?");
                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Update()
    {
        //destroy the object based on lifetime in case it doesn't destroy with any other collider for some reason
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            Destroy(this.gameObject);
        }
    }

}
