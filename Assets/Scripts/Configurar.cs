using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Configurar : MonoBehaviour
{
    public void SobreOJogo()
    {
        SceneManager.LoadScene("Sobreojogo");
    }

    public void ComoJogar()
    {
        SceneManager.LoadScene("Comojogar");
    }

    public void Voltar()
    {
        SceneManager.LoadScene("TelaInicio");
    }
     public void ChangeToCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void VoltarOptions()
    {
        SceneManager.LoadScene("Opcoes");
    }
    public void ChangeToConfigurations()
    {
        SceneManager.LoadScene("Configurations");
    }
}
