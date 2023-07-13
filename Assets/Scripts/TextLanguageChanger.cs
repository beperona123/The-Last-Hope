using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextLanguageChanger : MonoBehaviour 
{


	[TextArea(3,10)]
	public string portugueseText;
	[TextArea(3,10)]
	public string englishText;
	private Text text;
	void Start () 
	{
		text = GetComponent<Text>();
		if(PlayerPrefs.GetString("language", "Portuguese") == "English")
		{
			text.text = englishText;
		}
		else
		{
			text.text = portugueseText;
		}
	}
	
	void Update () 
	{
		if(PlayerPrefs.GetString("language", "Portuguese") == "English")
		{
			text.text = englishText;
		}
		else
		{
			text.text = portugueseText;
		}
		
	}
}
