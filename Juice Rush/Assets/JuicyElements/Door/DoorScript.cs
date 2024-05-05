using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public int particleCount = 0;
    [SerializeField] ParticleSystem _particleSystem;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetComponent<playerController>() != null)
        {
            transform.GetComponent<Animator>().SetBool("Open", true);
            transform.GetComponent<ParticleSystem>().Emit(particleCount * JuiceSlider.Instance.juiciness);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.GetComponent<playerController>() != null)
        {
            transform.GetComponent<Animator>().SetBool("Open", false);
        }
    }
}
