using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class swingAnimation : MonoBehaviour
{
    Material mat;
    Color color;
    float time;
    bool hit;
    bool firstFrame = true;
    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<Renderer>().material;
        time = 0;
        mat.color = new Color(1, 1, 1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        time += 2 * Time.deltaTime;
        if(this.gameObject.name == "Swing1")
        {
            color = mat.color;
            if (firstFrame)
            {
                color.a = 1f;
                firstFrame = false;
            }
            if (color.a < 1 && hit == false)
            {
                color.a += time;
            }
            else if (color.a >= 1)
            {
                hit = true;
                color.a -= time;
            }
            else if (color.a < 1 && hit == true)
            {
                color.a -= time;
            }
            mat.color = color;
            if (color.a <= 0 && hit == true)
            {
                Destroy(this.gameObject);
            }
        }
        else if(this.gameObject.name == "Swing2")
        {
            color = mat.color;
            if (firstFrame)
            {
                color.a = 0.8f;
                firstFrame = false;
            }
            if (color.a < 1 && hit == false)
            {
                color.a += time;
            }
            else if (color.a >= 1)
            {
                hit = true;
                color.a -= time;
            }
            else if (color.a < 1 && hit == true)
            {
                color.a -= time;
            }
            mat.color = color;
            if(color.a <= 0 && hit == true)
            {
                Destroy(this.gameObject);
            }
        }
        else if(this.gameObject.name == "Swing3")
        {
            color = mat.color;
            if (firstFrame)
            {
                color.a = 0.6f;
                firstFrame = false;
            }
            if (color.a < 1 && hit == false)
            {
                color.a += time;
            }
            else if (color.a >= 1)
            {
                hit = true;
                color.a -= time;
            }
            else if (color.a < 1 && hit == true)
            {
                color.a -= time;
            }
            mat.color = color;
            if (color.a <= 0 && hit == true)
            {
                Destroy(this.gameObject);
            }
        }
        else if(this.gameObject.name == "Swing4")
        {
            color = mat.color;
            if (firstFrame)
            {
                color.a = 0.4f;
                firstFrame = false;
            }
            if (color.a < 1 && hit == false)
            {
                color.a += time;
            }
            else if (color.a >= 1)
            {
                hit = true;
                color.a -= time;
            }
            else if (color.a < 1 && hit == true)
            {
                color.a -= time;
            }
            mat.color = color;
            if (color.a <= 0 && hit == true)
            {
                Destroy(this.gameObject);
            }
        }
        else if(this.gameObject.name == "Swing5")
        {
            color = mat.color;
            if (firstFrame)
            {
                color.a = 0.2f;
                firstFrame = false;
            }
            if (color.a < 1 && hit == false)
            {
                color.a += time;
            }
            else if (color.a >= 1)
            {
                hit = true;
                color.a -= time;
            }
            else if (color.a < 1 && hit == true)
            {
                color.a -= time;
            }
            mat.color = color;
            if (color.a <= 0 && hit == true)
            {
                Destroy(this.gameObject);
            }
        }
        else if (this.gameObject.name == "Swing6")
        {
            color = mat.color;
            if (firstFrame)
            {
                color.a = 0;
                firstFrame = false;
            }
            if (color.a < 1 && hit == false)
            {
                color.a += time;
            }
            else if (color.a >= 1)
            {
                hit = true;
                color.a -= time;
            }
            else if (color.a < 1 && hit == true)
            {
                color.a -= time;
            }
            mat.color = color;
            if (color.a <= 0 && hit == true)
            {
                Destroy(this.gameObject);
            }
        }
        else if (this.gameObject.name == "Swing7")
        {
            color = mat.color;
            if (firstFrame)
            {
                color.a = -0.2f;
                firstFrame = false;
            }
            if (color.a < 1 && hit == false)
            {
                color.a += time;
            }
            else if (color.a >= 1)
            {
                hit = true;
                color.a -= time;
            }
            else if (color.a < 1 && hit == true)
            {
                color.a -= 0.5f * time;
            }
            mat.color = color;
            if (color.a <= 0 && hit == true)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
