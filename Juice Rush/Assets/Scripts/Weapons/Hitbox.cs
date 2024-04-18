using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    List<GameObject> Colliders;
    public GameObject[] GameObjectColliders
    {
        get
        {
            return Colliders.ToArray();
        }
    }

    void Awake()
    {
        Colliders = new List<GameObject>();
    }

    void Update()
    {
        // Draw hitbox (Debugging purposes)
        Debug.DrawLine(transform.position, transform.position + transform.right * transform.localScale.x / 2, Color.blue);
        Debug.DrawLine(transform.position, transform.position - transform.right * transform.localScale.x / 2, Color.blue);
        Debug.DrawLine(transform.position, transform.position + transform.up * transform.localScale.y, Color.red);
        Debug.DrawLine(transform.position, transform.position - transform.up * transform.localScale.y, Color.red);
        Debug.DrawLine(transform.position, transform.position + transform.forward * transform.localScale.z / 2, Color.green);
        Debug.DrawLine(transform.position, transform.position - transform.forward * transform.localScale.z / 2, Color.green);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Enemy"))
        {
            Colliders.Add(other.gameObject);
            Debug.Log("Collided with: " + other.gameObject.name);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Enemy"))
        {
            Colliders.Remove(other.gameObject);
            Debug.Log("Exited collision with: " + other.gameObject.name);
        }
    }
}
