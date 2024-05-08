using System.Collections;
using UnityEngine;
public class katanaScript : MonoBehaviour
{
    float timer;
    float timer2;
    bool timerOn;
    bool chargedAttack;
    Quaternion rot;
    public GameObject animation1;
    public GameObject animation2;
    public int juiciness;
    public Material mat;
    [SerializeField] GameObject katana, Handle, Blade, BottomHandle, BladeGuard, HandleWrap;

    private void Awake()
    {
        katana.GetComponent<Animator>().SetBool("pulling", true);
    }
    public void StartAttack()
    {
        // katana.GetComponent<Animator>().Set("Hold");
        timer = 0;
        timerOn = true;
        if (katana.GetComponent<Animator>().GetFloat("timePassed") > 0.95f)
        {
            katana.GetComponent<Animator>().SetBool("swing1", false);
            katana.GetComponent<Animator>().SetBool("swing2", false);
            katana.GetComponent<Animator>().SetFloat("timePassed", 0);
        }
    }

    public void StopAttack()
    {
        if (!katana.GetComponent<Animator>().GetBool("swing1"))
        {
            rot = this.gameObject.transform.rotation;
            katana.GetComponent<Animator>().SetBool("swing1", true);
            katana.GetComponent<Animator>().SetBool("swing2", false);
            katana.GetComponent<Animator>().SetFloat("timePassed", 0);
            StartCoroutine(WaitOneTenth(animation1));
            ParticlesHit(timer);

            timerOn = false;
            timer2 = 0;
        }
        else if (katana.GetComponent<Animator>().GetBool("swing1"))
        {
            rot = this.gameObject.transform.rotation;
            katana.GetComponent<Animator>().SetBool("swing2", true);
            katana.GetComponent<Animator>().SetBool("swing1", false);
            katana.GetComponent<Animator>().SetFloat("timePassed", 0);
            StartCoroutine(WaitOneTenth(animation2));
            ParticlesHit(timer);
            timerOn = false;
            timer2 = 0;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        katana.GetComponent<Animator>().SetFloat("timePassed", 0);
        if (juiciness != 1 && juiciness != 2 && juiciness != 3)
        {
            juiciness = 3;
        }
        else if (juiciness == 1)
        {
            Handle.GetComponent<MeshRenderer>().material = mat;
            Blade.GetComponent<MeshRenderer>().material = mat;
            BottomHandle.GetComponent<MeshRenderer>().material = mat;
            BladeGuard.GetComponent<MeshRenderer>().material = mat;
            HandleWrap.GetComponent<MeshRenderer>().material = mat;
        }
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
    IEnumerator WaitOneTenth(GameObject animation)
    {
        yield return new WaitForSeconds(0.05f);
        if (juiciness == 3)
        {
            Instantiate(animation, this.gameObject.transform.position + new Vector3(0, -0.1f, 0), new Quaternion(rot.x, this.rot.y, rot.z, rot.w));
        }
    }
}
