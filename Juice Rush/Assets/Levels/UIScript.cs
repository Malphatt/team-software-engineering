using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIScript : MonoBehaviour
{
    [SerializeField] testingPlayerHealth health;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.GetComponent<TextMeshProUGUI>().text = "HEALTH: " + health.health;
    }
}
