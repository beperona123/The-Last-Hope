using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{

    public string cena;
   
    private void OnTriggerEnter2D(Collider2D troca)
    {
        if (troca.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(cena);
        } 
    }




}
