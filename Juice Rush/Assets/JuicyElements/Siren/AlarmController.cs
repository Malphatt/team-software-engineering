using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmController : MonoBehaviour
{
    public Light worldLight;
    public float intensity = 1;
    bool triggered = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TriggerAlarm()
    {
        if (!triggered)
        {
            StartCoroutine(SoundAlarmRoutine());
        }
    }

    IEnumerator SoundAlarmRoutine()
    {
        triggered = true;
        print("alarm started");
        Color oldColor = worldLight.color;
        float oldIntensity = worldLight.intensity;
        worldLight.color = Color.red;
        worldLight.intensity = JuiceSlider.Instance.juiciness;
        for (int i = 0; i < 10; i++) 
        {
            yield return new WaitForSeconds(1);
            print("sounding alarm! aaaah!");
            if (worldLight.color == Color.red)
            {
                worldLight.color = Color.yellow;
            }
            else
            {
                worldLight.color = Color.red;
            }
        }
        worldLight.color = oldColor;
        worldLight.intensity = oldIntensity;
        print("alarmStopped");
        triggered = false;
    }
}
