using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shotgunBullet : MonoBehaviour
{
    [SerializeField] float damage;
    //using a test player script
    private testingPlayerHealth tps;
    float lifeTime = 4f;
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Apply damage to a test player (current player controller has no health?)
            tps = other.gameObject.GetComponent<testingPlayerHealth>();
            if (tps != null)
            {
                tps.TakeDamage(damage);
                Debug.Log("Destroyed?");
                Destroy(gameObject);
            }
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
