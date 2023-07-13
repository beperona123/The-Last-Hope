using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//COLOCAR NO PLAYER

public class Interactable : MonoBehaviour
{
    public bool isInteractable { get; set; }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("Fire1"))
        {
            isInteractable = true;
        }
        else
        {
            isInteractable = false;
        }
        
    }
}
