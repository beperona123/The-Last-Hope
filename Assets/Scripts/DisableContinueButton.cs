using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisableContinueButton : MonoBehaviour 
{
	private Button thisButton;
	void Start()
	{
		thisButton = GetComponent<Button>();
	}

	void update()
	{
		if(SaveSystem.CheckContinueDisabled())
		{
			thisButton.interactable = false;
		}
		else
		{
			thisButton.interactable = true;
		}

	}
	
}
