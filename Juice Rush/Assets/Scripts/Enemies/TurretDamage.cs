using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretDamage : MonoBehaviour
{
    [SerializeField] float damage;
    //using a test player script
    private testingPlayerHealth tps;
    float lifeTime = 4f;
    void OnCollisionEnter(Collision other)
    {
        if (other.transform.GetComponent<testingPlayerHealth>() != null)
        {
            //Apply damage to player
            tps = other.gameObject.GetComponent<testingPlayerHealth>();
            if (tps != null)
            {
                tps.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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
