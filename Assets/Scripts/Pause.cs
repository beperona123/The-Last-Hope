using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public  bool gamePaused = false;
    private GameObject pauseMenuUI;
    private GameObject inGameUI;

    void Start()
    {
        pauseMenuUI = GameObject.Find("PauseMenu");
        inGameUI = GameObject.Find("In-Game");
        pauseMenuUI.SetActive(false);
        inGameUI.SetActive(true);
        gamePaused = false;

    }
    
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(gamePaused)
            {
                Resume();
            }
            else
            {
                if(!FindObjectOfType<Inventory>().inventoryOpen)
                {
                    Paused();
                }
                
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        inGameUI.SetActive(true);
        gamePaused = false;
        FindObjectOfType<CharacterControl>().talking = false;
        
    }

    public void Paused()
    {
        pauseMenuUI.SetActive(true);
        inGameUI.SetActive(false);
        gamePaused = true;
        FindObjectOfType<CharacterControl>().talking = true;
        
    }

    public void VoltarMenu()
    {
        SceneManager.LoadScene("TelaInicio");
    }
}
