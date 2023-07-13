using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaClimb : MonoBehaviour 
{	
	private Animator lavaClimbAnim;
	void Start () 
	{
		lavaClimbAnim = GetComponent<Animator>();
	}
	
	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			lavaClimbAnim.SetTrigger("lavaUp");
		}
	}
}
