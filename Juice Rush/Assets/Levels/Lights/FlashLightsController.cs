using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLightsController : MonoBehaviour
{
    bool lightFlashing = false;
    Light thisLight;
    ParticleSystem particles;
    float normalIntensity;
    private void Start()
    {
        thisLight = GetComponent<Light>();
        particles = GetComponent<ParticleSystem>();
        normalIntensity = thisLight.intensity;
    }
    private void FixedUpdate()
    {
        if (!lightFlashing)
        {
            StartCoroutine(FlashLightRoutine());
        }
    }

    IEnumerator FlashLightRoutine()
    {
        lightFlashing = true;
        yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
        thisLight.intensity = 0;
        particles.Play();
        yield return new WaitForSeconds(Random.Range(0.1f, 0.4f));
        thisLight.intensity = normalIntensity;
        lightFlashing = false;
    }
}
