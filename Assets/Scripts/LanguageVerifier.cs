using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageVerifier : MonoBehaviour 
{
	private Toggle englishToogle;
	private Toggle portugueseToogle;
	private Text englishText;
	private Text portugueseText;

	void Start () 
	{
		englishToogle = this.gameObject.transform.GetChild(0).GetComponent<Toggle>();
		englishText = this.gameObject.transform.GetChild(0).GetChild(1).GetComponent<Text>();
		portugueseToogle = this.gameObject.transform.GetChild(1).GetComponent<Toggle>();
		portugueseText = this.gameObject.transform.GetChild(1).GetChild(1).GetComponent<Text>();
		if(PlayerPrefs.GetString("language", "Portuguese") == "English")
		{
			englishToogle.isOn = true;
		}
		else
		{
			portugueseToogle.isOn = true;
		}

	}
	void Update () 
	{
		if(PlayerPrefs.GetString("language", "Portuguese") == "English")
		{
			englishText.text = "English";
			portugueseText.text = "Portuguese";
		}
		else
		{
			englishText.text = "Inglês";
			portugueseText.text = "Português";
		}
	}
	public void ChangeToPortugueseLanguage()
	{
		Language.ChangeToPortuguese();
	}
	public void ChangeToEnglishLanguage()
	{
		Language.ChangeToEnglish();
	}
}
