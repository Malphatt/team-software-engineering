using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaParticleSpawner : MonoBehaviour
{
    //Get the lava particle game object
    [SerializeField] GameObject lavaParticle;

    //Spawn rate and spawn time to manipulate the lava spawn
    [SerializeField] float spawnRate;
    float spawnTime;
    //Variables to control the min and max scale of the projectile (will be probably removed in the future)
    [SerializeField] Vector3 minLavaScale = new Vector3();
    [SerializeField] Vector3 maxLavaScale = new Vector3();
    [SerializeField] int minParticleAmount;
    [SerializeField] int maxParticleAmount;
    [SerializeField] float minForceMagnitude;
    [SerializeField] float maxForceMagnitude;
    


    // Update is called once per frame
    void Update()
    {
        if (Time.time > spawnTime)
        {
            LavaParticleSpawn();
            spawnTime = Time.time + spawnRate;
        }
    }

    void LavaParticleSpawn()
    {
        //Get the bounds of the area (box collider)
        Bounds area = GetComponent<BoxCollider>().bounds;
        //Get the random range of particles to be spawned at the same time (increase/decrease the range if needed)
        int particleAmount = Random.Range(minParticleAmount, maxParticleAmount);

        for (int i = 0; i < particleAmount; i++)
        {
            //Generate a random point in the area
            Vector3 randomPosition = new Vector3(
            Random.Range(area.min.x, area.max.x),
            area.max.y,
            Random.Range(area.min.z, area.max.z));




            GameObject particle = Instantiate(lavaParticle, randomPosition, Quaternion.identity);


            //Randomize the scale of the particle prefab
            Vector3 randomScale = new Vector3(
            Random.Range(minLavaScale.x, maxLavaScale.x),
            Random.Range(minLavaScale.y, maxLavaScale.y),
            Random.Range(minLavaScale.z, maxLavaScale.z));
            //Change the local scale of the particle to randomized scale
            particle.transform.localScale = randomScale;


            Rigidbody rb = particle.GetComponent<Rigidbody>();

            //Randomize the force magnitude and direction of lava being pushed up (increase the range if needed)
            float forceMagnitude = Random.Range(minForceMagnitude, maxForceMagnitude);
            Vector3 forceDirection = new Vector3(Random.Range(-0.15f, 0.15f), 1f, Random.Range(-0.15f, 0.15f));

            rb.AddForce(forceMagnitude * forceDirection, ForceMode.Impulse);
        }
    }
}
