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
        // Check if the object collided with is an enemy and doesn't already exist in the list of colliders
        if (other.gameObject.tag.Equals("Enemy") && !Colliders.Contains(other.gameObject))
            Colliders.Add(other.gameObject);
    }

    void OnTriggerExit(Collider other)
    {
        RemoveCollider(other.gameObject);
    }

    public void RemoveCollider(GameObject collider)
    {
        if (Colliders.Contains(collider))
            Colliders.Remove(collider);
    }
}
