using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Theme : MonoBehaviour
{
    public AudioSource audioSource;
    private List<GameObject> level1s = new List<GameObject>();
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Awake ()
    {
        level1s.Clear();
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Music");
        foreach(GameObject obj in objs)
            {
                if(obj.name == "Level1Theme")
                {
                   level1s.Add(obj);
                }
                if(obj.name == "Main Theme" ||obj.name == "Level2Theme")
                {
                    Destroy(obj);
                }
            }
        if(level1s.Count > 1)
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
