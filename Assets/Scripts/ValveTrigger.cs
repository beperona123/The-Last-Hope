using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValveTrigger : MonoBehaviour 
{

	private Animator valveAnim;
	private GameObject eIcon;
	private Animator eIconAnim;
	public YesOrNoText choiceIfRight;
	public YesOrNoText choiceIfLeft;
	public bool insideValveTrigger;
	private bool inRightChoice;
	void Start () 
	{
		eIcon = GameObject.Find("Player").transform.GetChild(1).gameObject;
		eIconAnim = eIcon.GetComponent<Animator>();
		valveAnim = GetComponent<Animator>();
		insideValveTrigger = false;
		valveAnim.SetBool("leftTurn", false);
		valveAnim.SetBool("rightTurn", false);
	}
	public void TriggerChoiceIfLeft ()
	{
		FindObjectOfType<YesOrNoManager>().StartYesOrNo(choiceIfLeft);
	}
	public void TriggerChoiceIfRight ()
	{
		FindObjectOfType<YesOrNoManager>().StartYesOrNo(choiceIfRight);
	}
	
	
	void Update () 
	{
		
		if(insideValveTrigger && FindObjectOfType<PlantTerminal>().terminalAnim.GetBool("passwordCorrect"))
		{
			eIcon.SetActive(true);
			if(FindObjectOfType<YesOrNoManager>().isInYesOrNo)
			{
				if(inRightChoice)
				{
					if(FindObjectOfType<YesOrNoManager>().yes )
					{
						valveAnim.SetBool("leftTurn", false);
						valveAnim.SetBool("rightTurn", true);
					}
				}
				else
				{
					if(FindObjectOfType<YesOrNoManager>().yes )
					{
						valveAnim.SetBool("leftTurn", true);
						valveAnim.SetBool("rightTurn", false);
					}
				}
			}
			
			if(Input.GetKeyDown(KeyCode.E) && !FindObjectOfType<CharacterControl>().talking)
			{
				eIconAnim.SetTrigger("triggered");
				if(!valveAnim.GetBool("leftTurn") && !valveAnim.GetBool("rightTurn"))
				{
					TriggerChoiceIfRight();
					inRightChoice = true;
				}
				else
				{
					if(!valveAnim.GetBool("leftTurn") && valveAnim.GetBool("rightTurn"))
					{
						TriggerChoiceIfLeft();
						inRightChoice = false;
					}
					else
					{
						if(valveAnim.GetBool("leftTurn") && !valveAnim.GetBool("rightTurn"))
						{
							TriggerChoiceIfRight();
							inRightChoice = true;
						}
					}
				}
			}
		}
	}
	void OnTriggerEnter2D(Collider2D other)
	{
	if (other.gameObject.tag =="Player" && !FindObjectOfType<ValveController>().valveComplete)
		{
			insideValveTrigger = true;
		}
	}
	void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.tag =="Player")
		{
			insideValveTrigger = false;
			eIcon.SetActive(false);
		}
	}
}
