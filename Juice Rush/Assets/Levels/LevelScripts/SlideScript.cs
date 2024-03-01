using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideScript : MonoBehaviour
{
    private void OnCollisionStay(Collision collision)
    {
        print("hi");
        if (collision.transform.tag == "Player")
        {
            print("hi");
            collision.transform.parent.GetComponent<CharacterController>().Move(new Vector3(1, 0, 0) * Time.deltaTime);
            collision.transform.parent.GetComponent<CharacterController>().Move(new Vector3(-1, 0, 0) * Time.deltaTime);
            collision.transform.parent.GetComponent<CharacterController>().Move(new Vector3(0, 0, 1) * Time.deltaTime);
            collision.transform.parent.GetComponent<CharacterController>().Move(new Vector3(0, 0, -1) * Time.deltaTime);
        }
    }
}
