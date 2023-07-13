using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantTerminal : MonoBehaviour {

	public Dialogue dialogueWithoutKey;
	public Dialogue dialogueWhenComplete;
	public YesOrNoText yesOrNoText;
	public Password password;
	private GameObject eIcon;
	private Animator eIconAnim;
	public Animator terminalAnim;
	private AudioSource audioSource;
	private bool isInsideTerminal;
	private bool keyInTerminal;
	public bool playerHasKey;
	void Start()
	{
		audioSource = GetComponent<AudioSource>();
		eIcon = GameObject.Find("Player").transform.GetChild(1).gameObject;
		terminalAnim = GetComponent<Animator>();
		eIconAnim = eIcon.GetComponent<Animator>();
		keyInTerminal = false;
	}
	public void TriggerTerminalDialogueWithoutKey ()
	{
		FindObjectOfType<DialogueManager>().StartDialogue(dialogueWithoutKey);
	}
	public void TriggerTerminalDialogueWhenComplete ()
	{
		FindObjectOfType<DialogueManager>().StartDialogue(dialogueWhenComplete);
	}
	public void TriggerTerminalChoice()
	{
		FindObjectOfType<YesOrNoManager>().StartYesOrNo(yesOrNoText);
	}
	public void TriggerTerminalPassword()
	{
		FindObjectOfType<PasswordManager>().StartPassword(password);
	}
	void Update () 
	{
		audioSource.volume = PlayerPrefs.GetFloat("fXVolume", 1);
		if(FindObjectOfType<CharacterControl>().HasItem("Key"))
		{
			playerHasKey = true;
		}
		else
		{
			playerHasKey = false;
		}
		if(isInsideTerminal)
		{
			eIcon.SetActive(true);
			if(FindObjectOfType<YesOrNoManager>().isInYesOrNo && FindObjectOfType<YesOrNoManager>().yes == true && FindObjectOfType<YesOrNoManager>().no == false && playerHasKey && !keyInTerminal)
			{
				FindObjectOfType<CharacterControl>().RemoveItem("Key");
				terminalAnim.SetBool("keyInside",true);
				terminalAnim.SetBool("passwordCorrect",false);
				keyInTerminal = true;
				audioSource.Play();
			}
			else
			{
				if(FindObjectOfType<PasswordManager>().passwording && FindObjectOfType<PasswordManager>().rightPassword && keyInTerminal)
				{
					terminalAnim.SetBool("passwordCorrect",true);
					Destroy(GameObject.Find("Water"));
					audioSource.Stop();
				}
			}
			if(Input.GetKeyDown(KeyCode.E) && !FindObjectOfType<CharacterControl>().talking)
			{
				eIconAnim.SetTrigger("triggered");
				if(!playerHasKey && !keyInTerminal)
				{
					TriggerTerminalDialogueWithoutKey();
				}
				else
				{
					if(playerHasKey && !keyInTerminal)
					{
						TriggerTerminalChoice();
					}	
				}
				if(keyInTerminal && !terminalAnim.GetBool("passwordCorrect"))
				{
					TriggerTerminalPassword();
				}
				else
				{
					if(terminalAnim.GetBool("passwordCorrect"))
					{
						TriggerTerminalDialogueWhenComplete();
					}
				}
			}
		}
	}
	void OnTriggerEnter2D(Collider2D other)
	{
		 if(other.gameObject.tag =="Player")
        {
            isInsideTerminal = true;
        }
	}
	void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.tag =="Player")
		{
			isInsideTerminal = false;
			eIcon.SetActive(false);
		}
	}
}
