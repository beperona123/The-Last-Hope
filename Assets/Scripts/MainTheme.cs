using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainTheme : MonoBehaviour
{
    private AudioSource audioSource;
    private List<GameObject> titles = new List<GameObject>();
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
    }

    void Awake ()
    {
        titles.Clear();
         GameObject[] objs = GameObject.FindGameObjectsWithTag("Music");
         foreach(GameObject obj in objs)
         {
             if(obj.name == "Main Theme")
             {
                 titles.Add(obj);
             }
             if(obj.name == "Level1Theme" || obj.name == "Level2Theme")
             {
                 Destroy(obj);
             }
         }
          if (titles.Count > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }
    void Update()
    {
       
        audioSource.volume = PlayerPrefs.GetFloat("sTVolume", 1);
    }
}
