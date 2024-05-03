using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shotgunBullet : MonoBehaviour
{
    [SerializeField] float damage;
    //using a test player script
    private testingPlayerHealth tps;
    float lifeTime = 3f;
    void OnCollisionEnter(Collision other)
    {
        if (other.transform.GetComponent<testingPlayerHealth>() != null)
        {
            //Apply damage to a test player
            tps = other.gameObject.GetComponent<testingPlayerHealth>();
            tps.TakeDamage(damage);
            Debug.Log("Destroyed?");
            Destroy(this.gameObject);
        }
        //When merged to main level, make an environment tag for walls, floor, etc
        //else if (other.gameObject.CompareTag("Environment"))
        //{
        //    Destroy(gameObject);
        //}

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
