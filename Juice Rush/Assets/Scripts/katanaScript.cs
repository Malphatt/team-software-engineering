using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class katanaScript : MonoBehaviour
{
    float timer;
    float timer2;
    bool timerOn;
    bool chargedAttack;
    public GameObject animation1;
    public GameObject animation2;
    [SerializeField] GameObject katana;

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            // katana.GetComponent<Animator>().Set("Hold");
            timer = 0;
            timerOn = true;
            if(katana.GetComponent<Animator>().GetFloat("timePassed") > 0.95f)
            {
                katana.GetComponent<Animator>().SetBool("swing1", false);
                katana.GetComponent<Animator>().SetBool("swing2", false);
                katana.GetComponent<Animator>().SetFloat("timePassed", 0);
            }
        }
        if (context.canceled && !katana.GetComponent<Animator>().GetBool("swing1"))
        {
            katana.GetComponent<Animator>().SetBool("swing1", true);
            Instantiate(animation1, this.gameObject.transform.position + new Vector3(0, -0.1f, 0), new Quaternion(this.gameObject.transform.rotation.x, this.gameObject.transform.rotation.y, this.gameObject.transform.rotation.z, this.gameObject.transform.rotation.w));
            katana.GetComponent<Animator>().SetBool("swing2", false);
            katana.GetComponent<Animator>().SetFloat("timePassed", 0);
            ParticlesHit(timer);

            timerOn = false;
            timer2 = 0;
        }
        else if (context.canceled && katana.GetComponent<Animator>().GetBool("swing1"))
        {
            katana.GetComponent<Animator>().SetBool("swing2", true);
            Instantiate(animation2, this.gameObject.transform.position + new Vector3(0, -0.1f, 0), new Quaternion(this.gameObject.transform.rotation.x, this.gameObject.transform.rotation.y, this.gameObject.transform.rotation.z, this.gameObject.transform.rotation.w));
            katana.GetComponent<Animator>().SetBool("swing1", false);
            katana.GetComponent<Animator>().SetFloat("timePassed", 0);
            ParticlesHit(timer);
            timerOn = false;
            timer2 = 0;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        katana.GetComponent<Animator>().SetFloat("timePassed", 0);
    }

    // Update is called once per frame
    void Update()
    {
         timer2 += Time.deltaTime;
        katana.GetComponent<Animator>().SetFloat("timePassed", timer2);
        if (katana.GetComponent<Animator>().GetFloat("timePassed") > 0.95f)
        {
            timer2 = 0;
            katana.GetComponent<Animator>().SetBool("swing1", false);
            katana.GetComponent<Animator>().SetBool("swing2", false);
        }
        if (timerOn)
        {
            timer += Time.deltaTime;
            if (timer > 0.5f)
            {
                ParticlesHold(timer);
                chargedAttack = true;
            }
            else
            {
                chargedAttack = false;
            }
        }
        else
        {
            timer = 0;
            chargedAttack = false;
        }
    }
    void ParticlesHold(float amount)
    {
        if (chargedAttack)
        {

        }
    }
    void ParticlesHit(float amount)
    {
        if (chargedAttack)
        {
            // Charged attack, timer amount of particles
        }
        else
        {
            // Normal attack
        }
    }
}
