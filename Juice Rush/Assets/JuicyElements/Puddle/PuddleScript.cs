using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PuddleScript : MonoBehaviour
{
    bool hasSplashed = false;
    void Splash()
    {
        if (!hasSplashed)
        {
            print("splash");
            StartCoroutine(SplashRoutine());
            transform.GetComponent<ParticleSystem>().Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetComponent<playerController>() != null)
        {
            ParticleSystem.ShapeModule _editableShape = transform.GetComponent<ParticleSystem>().shape;
            _editableShape.position = other.transform.position - transform.position;
            Splash();
        }
    }

    IEnumerator SplashRoutine()
    {
        hasSplashed = true;
        yield return new WaitForSeconds(1);
        hasSplashed = false;
    }
}
