using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class katanaScript : MonoBehaviour
{
    float timer;
    float timer2;
    bool timerOn;
    bool chargedAttack;
    [SerializeField] GameObject katana;

    public void Attack(InputAction.CallbackContext context)
    {
        Debug.Log("Attack");
        if (context.started)
        {
            // katana.GetComponent<Animator>().Set("Hold");
            timer = 0;
            timerOn = true;
        }
        if (context.canceled && !katana.GetComponent<Animator>().GetBool("swing1"))
        {
            katana.GetComponent<Animator>().SetBool("swing1", true);
            ParticlesHit(timer);
            timerOn = false;
            timer2 = 0;
        }
        else if (context.canceled && katana.GetComponent<Animator>().GetBool("swing1"))
        {
            katana.GetComponent<Animator>().SetBool("swing2", true);
            katana.GetComponent<Animator>().SetBool("swing1", false);
            ParticlesHit(timer);
            timerOn = false;
            timer2 = 0;
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer2 += Time.deltaTime;
        katana.GetComponent<Animator>().SetFloat("timePassed", timer2);
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
            Debug.Log("Charged Attack" + timer);
        }
        else
        {
            Debug.Log("Normal Attack" + timer);
        }
    }
}
