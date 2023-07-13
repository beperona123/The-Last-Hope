using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarregaCapitulo : MonoBehaviour
{
    [SerializeField]
    private GameObject cap1;
    [SerializeField]
    private GameObject cap2;

    private void CapituloPrimeiro()
    {
        SceneManager.LoadScene("Fase1parte1");
    }

    private void CapituloSegundo()
    {
        SceneManager.LoadScene("");
    }
    
}
