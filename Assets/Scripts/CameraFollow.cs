using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraFollow : MonoBehaviour
{
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
    private Transform player;

    // Use this for initialization
    void Start ()
    {
		player = GameObject.Find("Player").GetComponent<Transform>();
	}
	
	void Update ()
    { 
        transform.position = new Vector3(Mathf.Clamp(player.position.x, minX, maxX), Mathf.Clamp(player.position.y, minY, maxY), -10);
    }
}
