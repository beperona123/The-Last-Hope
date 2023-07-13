using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantDoor : MonoBehaviour 
{
	private Animator doorAnim;
	private GameObject eIcon;
	private Animator eIconAnim;
	private bool IsInside;
	void Start () 
	{
		doorAnim = GetComponentInParent<Animator>();
		eIcon = GameObject.Find("Player").transform.GetChild(1).gameObject;
		eIconAnim = eIcon.GetComponent<Animator>();
	}
	void Update ()
	{
		if(IsInside)
		{
			eIcon.SetActive(true);
			if(Input.GetKeyDown(KeyCode.E))
			{
				eIconAnim.SetTrigger("triggered");
				SwitchDoorAnim();
			}
		}
	}
	public void SwitchDoorAnim()
	{
		if(doorAnim.GetBool("doorOpen"))
		{
			doorAnim.SetBool("doorOpen", false);
		}
		else
		{
			doorAnim.SetBool("doorOpen", true);
		}
	}
	public void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
		{	
			IsInside = true;
		}
	}
	public void OnTriggerExit2D(Collider2D other)
	{
		if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			eIcon.SetActive(false);
			IsInside = false;
		}
	}
}
