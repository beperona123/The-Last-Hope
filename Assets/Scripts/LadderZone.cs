using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderZone : MonoBehaviour 
{
	private CharacterControl playerScript;
	private BoxCollider2D topBoxCol;
	private PolygonCollider2D topPolyCol;
	private Animator eIconAnim;
	private GameObject eIcon;
	public bool onLadder;
	void Start () 
	{
		playerScript = FindObjectOfType<CharacterControl>();
		if(this.transform.GetChild(0).GetComponent<BoxCollider2D>() != null)
		{
			topBoxCol = this.transform.GetChild(0).GetComponent<BoxCollider2D>();
		}
		else
		{
			topPolyCol = this.transform.GetChild(0).GetComponent<PolygonCollider2D>();
		}
		eIcon = GameObject.Find("Player").transform.GetChild(1).gameObject;
		eIconAnim = eIcon.GetComponent<Animator>();
		onLadder = false;
	}
	
	void Update () 
	{
		if(onLadder)
		{
			eIcon.SetActive(true);
			if(Input.GetKeyDown(KeyCode.E) && !playerScript.isClimbingLadder)
			{
				playerScript.isClimbingLadder = true;
			}
			else
			{
				if(Input.GetKeyDown(KeyCode.E) && playerScript.isClimbingLadder)
				{
					playerScript.isClimbingLadder = false;
					eIconAnim.SetBool("pressed", false);
				}
			}
		
			if(playerScript.isClimbingLadder)
			{
				eIconAnim.SetBool("pressed", true);
				if(topBoxCol != null)
				{
					topBoxCol.enabled = false;
				}
				else
				{
					topPolyCol.enabled = false;
				}
			}
			if(!playerScript.isClimbingLadder)
			{
				if(topBoxCol != null)
				{
					topBoxCol.enabled = true;
				}
				else
				{
					topPolyCol.enabled = true;
				}
			}
		}
		
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if(other.gameObject.tag == "Player")
		{
			onLadder = true;
		}
		if(topBoxCol != null)
			{
				topBoxCol.enabled = true;
			}
			else
			{
				topPolyCol.enabled = true;
			}
	}
	void OnTriggerExit2D (Collider2D other)
	{
		if(other.gameObject.tag == "Player")
		{
			onLadder = false;
			playerScript.isClimbingLadder = false;
			eIconAnim.SetBool("pressed", false);
			eIcon.SetActive(false);
			if(topBoxCol != null)
			{
				topBoxCol.enabled = true;
			}
			else
			{
				topPolyCol.enabled = true;
			}
		}
	}
}
