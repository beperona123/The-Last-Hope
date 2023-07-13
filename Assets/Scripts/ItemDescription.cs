using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDescription : MonoBehaviour {

	public Dialogue description;
	public void TriggerDescription()
	{
		FindObjectOfType<DialogueManager>().StartDialogue(description);
	}
}
