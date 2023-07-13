using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Language : MonoBehaviour 
{
	public static bool isEnglish = false;

	public static void ChangeToEnglish ()
	{
		isEnglish = true;
		PlayerPrefs.SetString("language", "English");
	}
		public static void ChangeToPortuguese ()
	{
		isEnglish = false;
		PlayerPrefs.SetString("language", "Portuguese");
	}
}
