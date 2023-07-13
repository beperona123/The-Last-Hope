using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalBoxActive : MonoBehaviour 
{
	void Update () 
	{
		if(GameObject.Find("Plant Terminal").GetComponent<Animator>().GetBool("passwordCorrect"))
		{
			transform.gameObject.tag = "ObjMover";
		}
		
	}
}
