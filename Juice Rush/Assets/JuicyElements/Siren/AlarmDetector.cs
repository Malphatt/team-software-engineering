using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmDetector : MonoBehaviour
{
    public AlarmController alarm;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetComponent<playerController>() != null)
        {
            alarm.TriggerAlarm();
        }
    }
}
