using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class menuManager : MonoBehaviour
{
    public GameObject controlsPanel;

    public void NextScene()
    {
        //Load the next scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
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
