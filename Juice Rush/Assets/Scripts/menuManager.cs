using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class menuManager : MonoBehaviour
{
    public GameObject controlsPanel;
    public GameObject juiceSlider;

    public void NextScene()
    {
        //Get the value of the slider
        float juiceValue = juiceSlider.GetComponent<UnityEngine.UI.Slider>().value;

        if (juiceValue == 0)
        {
            //Load the "Juiceless scene"
            UnityEngine.SceneManagement.SceneManager.LoadScene("Juiceless");
        }
        else
        {
            //Load the "Juicy" scene
            UnityEngine.SceneManagement.SceneManager.LoadScene("Juicy");
        }
    }



    public void togglePauseMenu(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Debug.Log("Toggle Pause Menu");
        }
    }

    public void toggleControls()
    {
        //Toggle the controls panel
        controlsPanel.SetActive(!controlsPanel.activeSelf);
    }

    public void QuitGame()
    {
        //Quit the game
        Application.Quit();
    }
}
