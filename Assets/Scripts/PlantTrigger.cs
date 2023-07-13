using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantTrigger : MonoBehaviour 
{
	private Animator plantAnim;

	void Start () 
	{
		plantAnim = GetComponent<Animator>();
		plantAnim.SetBool("visible", true);
	}
	
	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.tag == "Player"|| other.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			plantAnim.SetBool("visible", false);
		}
	}
	void OnTriggerExit2D(Collider2D other)
	{
		if(other.gameObject.tag == "Player"|| other.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			
			plantAnim.SetBool("visible", true);
		}
	}
}
