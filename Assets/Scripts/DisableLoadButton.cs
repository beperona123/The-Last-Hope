using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisableLoadButton : MonoBehaviour 
{
	private Button thisButton;
	void Start()
	{
		thisButton = GetComponent<Button>();
		if(SaveSystem.CheckChaptersDisabled() != null)
		{
			if(this.gameObject.name == "Chapter 2")
			{
				if(SaveSystem.CheckChaptersDisabled().lastCheckpoint == "Chapter 2 - Lava" || SaveSystem.CheckChaptersDisabled().lastCheckpoint == "Chapter 2 - Power Plant")
				{
					thisButton.interactable = true;	
				}
				else
				{
					thisButton.interactable = false;	
				}
			}
			else
			{
				if(this.gameObject.name == "Chapter 1")
				{
					if(SaveSystem.CheckChaptersDisabled().lastCheckpoint == "Fase1parte1" || SaveSystem.CheckChaptersDisabled().lastCheckpoint == "FaseTutorial" || SaveSystem.CheckChaptersDisabled().lastCheckpoint == "Fase1parte2" || SaveSystem.CheckChaptersDisabled().lastCheckpoint == "Fase1parte3" || SaveSystem.CheckChaptersDisabled().lastCheckpoint == "Fase1parte4" || SaveSystem.CheckChaptersDisabled().lastCheckpoint == "Fase1parte5" || SaveSystem.CheckChaptersDisabled().lastCheckpoint == "Fase1parte6" || SaveSystem.CheckChaptersDisabled().lastCheckpoint == "Fase1final" || SaveSystem.CheckChaptersDisabled().lastCheckpoint == "Fase1saida")
					{
						thisButton.interactable = true;	
					}
					else
					{
						thisButton.interactable = false;	
					}
				}
				else
				{
					if(this.gameObject.name == "Load")
					{
						thisButton.interactable = true;
					}
				}
				
			}
		}
		else
		{
			thisButton.interactable = false;	
		}
	}

}
