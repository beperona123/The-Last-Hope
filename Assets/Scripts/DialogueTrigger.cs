using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

	public Dialogue dialogue;
	private GameObject eIcon;
	private Animator eIconAnim;
	private bool alreadyShowed = false;
	private bool isInsideTextItem;

	public void TriggerDialogue ()
	{
		FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
	}
	void Start()
	{
		eIcon = GameObject.Find("Player").transform.GetChild(1).gameObject;
		eIconAnim = eIcon.GetComponent<Animator>();
	}
	void Update()
	{
		if(isInsideTextItem)
		{
			eIcon.SetActive(true);
			if(Input.GetKeyDown(KeyCode.E) && !FindObjectOfType<CharacterControl>().talking)
			{
				if(!FindObjectOfType<CharacterControl>().talking)
				eIconAnim.SetTrigger("triggered");
				TriggerDialogue();
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		 if(other.gameObject.tag =="Player" && !alreadyShowed && this.gameObject.tag != "Text Item")
        {
            TriggerDialogue();
			alreadyShowed = true;
        }
		else
		{
			if(other.gameObject.tag =="Player" && this.gameObject.tag == "Text Item")
			{
				isInsideTextItem = true;
			}
		}
	}
	void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.tag =="Player" && this.gameObject.tag == "Text Item")
		{
			isInsideTextItem = false;
			eIcon.SetActive(false);
		}
	}
}
