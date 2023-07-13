using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour 
{

	public Queue<string> sentences;
	public Queue<string> names;
	private Text dialogueText;
	private Text nameText;
	private Image speakerImage;
	public Sprite manibusImage;
	public Sprite CassiusImage;
	private Animator animator;
	[HideInInspector]
	public bool dialoguing;
	public float delay = 0.2f;
	void Start () 
	{
		sentences = new Queue<string>();
		names = new Queue<string>();
		animator = GameObject.Find("Box").GetComponent<Animator>();
		speakerImage = GameObject.Find("ID").transform.GetChild(0).GetComponent<Image>();
		dialogueText = GameObject.Find("Dialogue Box").transform.GetChild(0).GetComponent<Text>();
		nameText = GameObject.Find("ID").transform.GetChild(1).GetComponent<Text>();
	}

	void Update()
	{
		if((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return)) && dialoguing == true && animator.GetBool("isOpen") == true)
		{
			DisplayNextSentence();
		}
	}

	public void StartDialogue(Dialogue dialogue)
	{
		dialoguing = true;
		GameObject.Find("Player").GetComponent<CharacterControl>().talking = true;
		animator.SetBool("isOpen",true);
		names.Clear();
		sentences.Clear();
		if(PlayerPrefs.GetString("language", "Portuguese") == "Portuguese")
		{
			foreach(string sentence in dialogue.portugueseSentences)
			{
				sentences.Enqueue(sentence);
			}
			foreach(string name in dialogue.names)
			{
				names.Enqueue(name);
			}
		}
		else
		{
			foreach(string sentence in dialogue.englishSentences)
			{
				sentences.Enqueue(sentence);
			}
			foreach(string name in dialogue.names)
			{
				names.Enqueue(name);
			}
			
		}
		StopAllCoroutines();
		DisplayNextSentence();
	}

	public void DisplayNextSentence()
	{
		if(dialoguing == true)
		{
			if(sentences.Count == 0)
			{
				StartCoroutine(EndDialogue());
				return;
			}
			string sentence = sentences.Dequeue();
			string name = names.Dequeue();
			StopAllCoroutines();
			StartCoroutine(TypeSentence(sentence,name));
		}
	}

	IEnumerator TypeSentence (string sentence, string name)
	{
		if(name == "Cassius" || name == "cassius")
		{
			animator.SetBool("isTextOpen",false);
			animator.SetBool("isPasswordOpen",false);
			speakerImage.sprite = CassiusImage;
		}
		else
		{
			if(name == "Manibus" || name == "manibus")
			{
				animator.SetBool("isTextOpen",false);
				animator.SetBool("isPasswordOpen",false);
				speakerImage.sprite = manibusImage;
			}
			else
			{
				animator.SetBool("isTextOpen",true);
				animator.SetBool("isPasswordOpen",false);
			}
		}
		nameText.text="";
		dialogueText.text = "";
		foreach(char letter in name.ToCharArray())
		{
			nameText.text += letter;
			yield return null;
		}
		foreach(char letter in sentence.ToCharArray())
		{
			dialogueText.text += letter;
			yield return new WaitForSeconds(delay);
		}
	}

	IEnumerator EndDialogue()
	{
		yield return null;
		StopAllCoroutines();
		dialoguing = false;
		animator.SetBool("isOpen",false);
		GameObject.Find("Player").GetComponent<CharacterControl>().talking = false;
	}
}
