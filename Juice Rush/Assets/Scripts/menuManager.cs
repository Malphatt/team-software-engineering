using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuManager : MonoBehaviour
{
    public GameObject controlsPanel;
    public void NextScene()
    {
        //Load the next scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
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
