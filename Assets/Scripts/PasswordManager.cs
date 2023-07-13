using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PasswordManager : MonoBehaviour {

	
	public Queue<string> sentencesBefore;
	private string sentenceDuring;
	public Queue<string> sentencesAfterRight;
	public Queue<string> sentencesAfterWrong;
	private string passwordTitle;
	private string Shownsentence;
	private int initialAfterRightCount;
	private int initialAfterWrongCount;
	private Text dialogueText;
	private Animator animator;
	private InputField passwordInput;
	[HideInInspector]
	public bool passwording;
	[HideInInspector]
	public bool rightPassword;
	public float delay = 0.2f;
	void Start () 
	{
		sentencesBefore = new Queue<string>();
		sentencesAfterRight = new Queue<string>();
		sentencesAfterWrong = new Queue<string>();
		animator = GameObject.Find("Box").GetComponent<Animator>();
		dialogueText = GameObject.Find("Dialogue Box").transform.GetChild(0).GetComponent<Text>();
		passwordInput = GameObject.Find("Dialogue Box").transform.GetChild(2).GetComponent<InputField>();
	}

	void Update()
	{
		if((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return)) && passwording == true && animator.GetBool("isOpen") == true)
		{
			DisplayNextPasswordSentence();
		}
		passwordInput.text = passwordInput.text.ToUpper();
	}

	public void StartPassword(Password password)
	{	
		passwording = true;
		GameObject.Find("Player").GetComponent<CharacterControl>().talking = true;
		sentencesBefore.Clear();
		sentencesAfterRight.Clear();
		sentencesAfterWrong.Clear();
			if(PlayerPrefs.GetString("language", "Portuguese") == "Portuguese")
		{
			initialAfterRightCount = password.portugueseSentencesAfterRight.Length;
			initialAfterWrongCount = password.portugueseSentencesAfterWrong.Length;
			passwordTitle = password.passwordName;
			sentenceDuring = password.portugueseSentenceDuring;
			foreach(string sentence in password.portugueseSentencesBefore)
			{
				sentencesBefore.Enqueue(sentence);
			}
			foreach(string sentence in password.portugueseSentencesAfterRight)
			{
				sentencesAfterRight.Enqueue(sentence);
			}
			foreach(string sentence in password.portugueseSentencesAfterWrong)
			{
				sentencesAfterWrong.Enqueue(sentence);
			}
		}
		else
		{
			initialAfterRightCount = password.englishSentencesAfterRight.Length;
			initialAfterWrongCount = password.englishSentencesAfterWrong.Length;
			passwordTitle = password.passwordName;
			sentenceDuring = password.englishSentenceDuring;
			foreach(string sentence in password.englishSentencesBefore)
			{
				sentencesBefore.Enqueue(sentence);
			}
			foreach(string sentence in password.englishSentencesAfterRight)
			{
				sentencesAfterRight.Enqueue(sentence);
			}
			foreach(string sentence in password.englishSentencesAfterWrong)
			{
				sentencesAfterWrong.Enqueue(sentence);
			}
		}
		StopAllCoroutines();
		DisplayNextPasswordSentence();
	}

	public void DisplayNextPasswordSentence()
	{
		if(passwording == true)
		{
			if(passwordInput.text == passwordTitle)
			{
				rightPassword = true;
			}
			else
			{
				rightPassword = false;
			}
			if((((sentencesBefore.Count + sentencesAfterRight.Count == 0) || (sentencesBefore.Count + sentencesAfterWrong.Count == 0)) && Shownsentence != sentenceDuring && Shownsentence != null) || (Shownsentence == sentenceDuring && sentencesBefore.Count== 0 && (sentencesAfterRight.Count == 0 || sentencesAfterWrong.Count ==0)))
			{
				StartCoroutine(EndPasswordDialogue());
				return;
			}
			if(sentencesBefore.Count != 0)
			{
				animator.SetBool("isOpen",true);
				animator.SetBool("isPasswordOpen",false);
				animator.SetBool("isTextOpen",true);
				Shownsentence = sentencesBefore.Dequeue();
			}
			else
			{
				if(sentencesBefore.Count == 0 && sentencesAfterRight.Count == initialAfterRightCount && sentencesAfterWrong.Count == initialAfterWrongCount)
				{
					animator.SetBool("isOpen",true);
					animator.SetBool("isPasswordOpen",true);
					animator.SetBool("isTextOpen",false);
					Shownsentence = sentenceDuring;
					initialAfterRightCount--;
					initialAfterRightCount--;
				}
				else
				{
					animator.SetBool("isOpen",true);
					animator.SetBool("isPasswordOpen",false);
					animator.SetBool("isTextOpen",true);
					if(passwordInput.text == passwordTitle)
					{
						Shownsentence = sentencesAfterRight.Dequeue();
						rightPassword = true;
					}
					else
					{
						Shownsentence = sentencesAfterWrong.Dequeue();
						rightPassword = false;
					}
				}
			}
			StopAllCoroutines();
			StartCoroutine(TypePasswordSentence(Shownsentence));
		}
	}

	IEnumerator TypePasswordSentence (string sentence)
	{
		dialogueText.text = "";
		foreach(char letter in sentence.ToCharArray())
		{
			dialogueText.text += letter;
			yield return null;
		}
	}


	IEnumerator EndPasswordDialogue()
	{
		yield return null;
		StopAllCoroutines();
		passwording = false;
		Shownsentence = "";
		animator.SetBool("isOpen",false);
		GameObject.Find("Player").GetComponent<CharacterControl>().talking = false;
	}
}
