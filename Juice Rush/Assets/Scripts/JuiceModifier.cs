using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JuiceModifier : MonoBehaviour
{
    public GameObject juiceSlider;
    public GameObject juiceText;
    
    private int juiceLevel;
    private Vector3 juiceTextPosition;
    // Start is called before the first frame update
    void Start()
    {
        //Set the current position
        juiceTextPosition = juiceText.transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        //Get the current juice level
        juiceLevel = ((int)juiceSlider.GetComponent<UnityEngine.UI.Slider>().value);

        //Change the textmeshpro of this object to the current juice level
        juiceText.GetComponent<TMPro.TextMeshProUGUI>().text = juiceLevel.ToString();

        //Set the speed of the animation to the current juice level
        juiceText.GetComponent<Animator>().speed = (juiceLevel)*2;

        //If the juice level is 0, reset the animation
        if (juiceLevel == 0)
        {
            juiceText.GetComponent<Animator>().Play("Juice Speed", 0, 0);
        }

        //Set the size of the text to the current juice level
        juiceText.transform.localScale = new Vector3(juiceLevel*0.2f+1, juiceLevel * 0.2f + 1, 1);
        juiceText.transform.position = juiceTextPosition + new Vector3(0, juiceLevel, 0);
    }
}
