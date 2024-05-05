using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PuddleScript : MonoBehaviour
{
    bool hasSplashed = false;
    public int particleCount = 0;
    [SerializeField] ParticleSystem _particleSystem;
    void Splash()
    {
        if (!hasSplashed)
        {
            print("splash");
            StartCoroutine(SplashRoutine());
            var main = _particleSystem.main;
            main.startSpeed = new ParticleSystem.MinMaxCurve(JuiceSlider.Instance.juiciness * 0.5f, JuiceSlider.Instance.juiciness);
            transform.GetComponent<ParticleSystem>().Emit(particleCount * JuiceSlider.Instance.juiciness);
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

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.GetComponent<playerController>() != null)
        {
            if (other.transform.GetComponent<playerController>().inputDirection != Vector3.zero)
            {
                ParticleSystem.ShapeModule _editableShape = transform.GetComponent<ParticleSystem>().shape;
                _editableShape.position = other.transform.position - transform.position;
                Splash();
            }
        }
    }

    IEnumerator SplashRoutine()
    {
        hasSplashed = true;
        yield return new WaitForSeconds(0.3f);
        hasSplashed = false;
    }
}
