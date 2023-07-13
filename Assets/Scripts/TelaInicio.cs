using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TelaInicio : MonoBehaviour
{
    public GameObject start;
    public GameObject continuar;
    public GameObject options;
    public GameObject load;
    public GameObject quit;

    public void Inicio()
    {
        SceneManager.LoadScene("FaseTutorial");

    }

    public void Carregar()
    {
        SceneManager.LoadScene("Load");
    }

    public void Config()
    {
        SceneManager.LoadScene("Opcoes");
    }

    public void CloseApplication()
    {
        Application.Quit();
    }
}
