using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YesOrNoManager : MonoBehaviour {

	public Queue<string> sentencesBefore;
	private string sentenceDuring;
	public Queue<string> sentencesAfterYes;
	public Queue<string> sentencesAfterNo;
	private string Shownsentence;
	private int initialAfterYesCount;
	private int initialAfterNoCount;
	private Text dialogueText;
	private Animator animator;
	[HideInInspector]
	public bool isInYesOrNo;
	[HideInInspector]
	public bool yes;
	[HideInInspector]
	public bool no;
	public float delay = 0.2f;
	void Start () 
	{
		sentencesBefore = new Queue<string>();
		sentencesAfterYes = new Queue<string>();
		sentencesAfterNo = new Queue<string>();
		animator = GameObject.Find("Box").GetComponent<Animator>();
		dialogueText = GameObject.Find("Dialogue Box").transform.GetChild(0).GetComponent<Text>();
	}

	void Update()
	{
		if((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return)) && isInYesOrNo == true && animator.GetBool("isChoice") == false && animator.GetBool("isOpen") == true )
		{
			DisplayNextYesOrNoSentence();
		}
	}

	public void StartYesOrNo(YesOrNoText yesOrNo)
	{	
		isInYesOrNo = true;
		yes = false;
		no = false;
		sentencesBefore.Clear();
		sentencesAfterYes.Clear();
		sentencesAfterNo.Clear();
		GameObject.Find("Player").GetComponent<CharacterControl>().talking = true;
		if(PlayerPrefs.GetString("language", "Portuguese") == "Portuguese")
		{
			initialAfterYesCount = yesOrNo.portugueseSentencesAfterYes.Length;
			initialAfterNoCount = yesOrNo.portugueseSentencesAfterNo.Length;
			sentenceDuring = yesOrNo.portugueseSentenceDuring;
			foreach(string sentence in yesOrNo.portugueseSentencesBefore)
			{
				sentencesBefore.Enqueue(sentence);
			}
			foreach(string sentence in yesOrNo.portugueseSentencesAfterYes)
			{
				sentencesAfterYes.Enqueue(sentence);
			}
			foreach(string sentence in yesOrNo.portugueseSentencesAfterNo)
			{
				sentencesAfterNo.Enqueue(sentence);
			}
		}
		else
		{
			initialAfterYesCount = yesOrNo.englishSentencesAfterYes.Length;
			initialAfterNoCount = yesOrNo.englishSentencesAfterNo.Length;
			sentenceDuring = yesOrNo.englishSentenceDuring;
			foreach(string sentence in yesOrNo.englishSentencesBefore)
			{
				sentencesBefore.Enqueue(sentence);
			}
			foreach(string sentence in yesOrNo.englishSentencesAfterYes)
			{
				sentencesAfterYes.Enqueue(sentence);
			}
			foreach(string sentence in yesOrNo.englishSentencesAfterNo)
			{
				sentencesAfterNo.Enqueue(sentence);
			}
		}
		StopAllCoroutines();
		DisplayNextYesOrNoSentence();
		
	}

	public void DisplayNextYesOrNoSentence()
	{
		if(isInYesOrNo == true)
		{
			if((((sentencesBefore.Count + sentencesAfterYes.Count == 0) || (sentencesBefore.Count + sentencesAfterNo.Count == 0)) && Shownsentence != sentenceDuring && Shownsentence != null) || (Shownsentence == sentenceDuring && sentencesBefore.Count == 0 && (sentencesAfterNo.Count == 0 || sentencesAfterYes.Count == 0)))
			{
				StartCoroutine(EndYesOrNoDialogue());
				return;
			}
			if(sentencesBefore.Count != 0)
			{
				animator.SetBool("isOpen",true);
				animator.SetBool("isChoice",false);
				animator.SetBool("isTextOpen",true);
				Shownsentence = sentencesBefore.Dequeue();
			}
			else
			{
				if(sentencesBefore.Count == 0 && sentencesAfterYes.Count == initialAfterYesCount && sentencesAfterNo.Count == initialAfterNoCount)
				{
					animator.SetBool("isOpen",true);
					animator.SetBool("isChoice",true);
					animator.SetBool("isTextOpen",false);
					Shownsentence = sentenceDuring;
					initialAfterYesCount--;
					initialAfterNoCount--;
				}
				else
				{
					if(yes && !no)
					{
						Shownsentence = sentencesAfterYes.Dequeue();
					}
					else
					{
						if(no && !yes)
						{
						Shownsentence = sentencesAfterNo.Dequeue();
						}
					}
				}
			}
			StopAllCoroutines();
			StartCoroutine(TypeYesOrNoSentence(Shownsentence));
			
		}
	}
	public void DisplayYesSentences()
	{	
		yes = true;
		no = false;
		if((((sentencesBefore.Count + sentencesAfterYes.Count == 0 ) || (sentencesBefore.Count + sentencesAfterNo.Count == 0)) && Shownsentence != sentenceDuring && Shownsentence != null) || (Shownsentence == sentenceDuring && sentencesBefore.Count == 0 && (sentencesAfterNo.Count == 0 || sentencesAfterYes.Count == 0)))
		{
			StartCoroutine(EndYesOrNoDialogue());
			return;
		}
		else
		{
			animator.SetBool("isOpen",true);
			animator.SetBool("isChoice",false);
			animator.SetBool("isTextOpen",true);
			Shownsentence = sentencesAfterYes.Dequeue();
			StopAllCoroutines();
			StartCoroutine(TypeYesOrNoSentence(Shownsentence));
		}
		
	}
	public void DisplayNoSentences()
	{
		yes = false;
		no = true;
		if((((sentencesBefore.Count + sentencesAfterYes.Count == 0 ) || (sentencesBefore.Count + sentencesAfterNo.Count == 0)) && Shownsentence != sentenceDuring && Shownsentence != null) || (Shownsentence == sentenceDuring && sentencesBefore.Count == 0 && (sentencesAfterNo.Count == 0 || sentencesAfterYes.Count == 0)))
		{
			StartCoroutine(EndYesOrNoDialogue());
			return;
		}
		else
		{
			animator.SetBool("isOpen",true);
			animator.SetBool("isChoice",false);
			animator.SetBool("isTextOpen",true);
			Shownsentence = sentencesAfterNo.Dequeue();
			StopAllCoroutines();
			StartCoroutine(TypeYesOrNoSentence(Shownsentence));
		}
		
	}

	IEnumerator TypeYesOrNoSentence (string sentence)
	{
		dialogueText.text = "";
		foreach(char letter in sentence.ToCharArray())
		{
			dialogueText.text += letter;
			yield return null;
		}
		
	}


	IEnumerator EndYesOrNoDialogue()
	{
		yield return null;
		StopAllCoroutines();
		Shownsentence = "";
		isInYesOrNo = false;
		yes = false;
		no = false;
		animator.SetBool("isOpen",false);
		GameObject.Find("Player").GetComponent<CharacterControl>().talking = false;
		
	}
}
