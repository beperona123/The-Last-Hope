using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtonController : MonoBehaviour
{
    private GameObject _startButtonObj;
    private GameObject _continueButtonObj;
    private GameObject _optionsButtonObj;
    private GameObject _loadButtonObj;
    private GameObject _quitButtonObj;

    public void StartGame()
    {
        SceneManager.LoadScene("FaseTutorial");
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene("Load");
    }

    public void LoadConfigScreen()
    {
        SceneManager.LoadScene("Opcoes");
    }

    public void ExitGameWindow()
    {
        Application.Quit();
    }
}
