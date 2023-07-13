using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerTrigger : MonoBehaviour {

	private Animator towerAnim;

	void Start () 
	{
		towerAnim = GetComponent<Animator>();
		towerAnim.SetBool("visible", true);
	}
	
	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.tag == "Player" || other.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			towerAnim.SetBool("visible", false);
		}
	}
	void OnTriggerExit2D(Collider2D other)
	{
		if(other.gameObject.tag == "Player" || other.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			
			towerAnim.SetBool("visible", true);
		}
	}
}
