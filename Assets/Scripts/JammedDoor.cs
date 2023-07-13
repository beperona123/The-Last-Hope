using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JammedDoor : MonoBehaviour 
{
	private PlantDoor doorScript;
	private DialogueTrigger textTriggerScript;
	void Start () 
	{
		doorScript = this.gameObject.GetComponent<PlantDoor>();
		textTriggerScript = this.gameObject.GetComponent< DialogueTrigger>();
		doorScript.enabled = false;
		textTriggerScript.enabled = true;

	}
	
	void Update () 
	{
		if(GameObject.Find("Plant Terminal").GetComponent<Animator>().GetBool("passwordCorrect"))
		{
			doorScript.enabled = true;
			textTriggerScript.enabled = false;
		}
	}
}
