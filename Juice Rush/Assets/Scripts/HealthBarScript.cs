using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarScript : MonoBehaviour
{
    [SerializeField] testingPlayerHealth health;
    // Start is called before the first frame update
    private void FixedUpdate()
    {
        Vector3 scale = transform.localScale;
        scale.x = health.health / 100;
        transform.localScale = scale;
    }
}
