using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // print("hi");
        if (other.transform.GetComponent<playerController>() != null)
        {
            transform.GetComponent<Animator>().SetBool("Open", true);
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
