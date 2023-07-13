using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValveController : MonoBehaviour {

	private Animator valve1Anim;
	private Animator valve2Anim;
	private Animator valve3Anim;
	private Animator valve4Anim;
	private Animator valve5Anim;
	private Animator valve6Anim;
	private ParticleSystem steamPipeParticle;
	[HideInInspector]
	public bool valveComplete;
	void Start () 
	{
		steamPipeParticle= GameObject.Find("Steam Pipe").transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
		valve1Anim = this.transform.GetChild(0).GetChild(0).GetComponent<Animator>();
		valve2Anim = this.transform.GetChild(1).GetChild(0).GetComponent<Animator>();
		valve3Anim = this.transform.GetChild(2).GetChild(0).GetComponent<Animator>();
		valve4Anim = this.transform.GetChild(3).GetChild(0).GetComponent<Animator>();
		valve5Anim = this.transform.GetChild(4).GetChild(0).GetComponent<Animator>();
		valve6Anim = this.transform.GetChild(5).GetChild(0).GetComponent<Animator>();
		valveComplete = false;

	}
	
	// Update is called once per frame
	void Update () 
	{
		if(valve1Anim.GetBool("leftTurn") && valve2Anim.GetBool("leftTurn") && valve3Anim.GetBool("rightTurn") && valve4Anim.GetBool("leftTurn") && valve5Anim.GetBool("rightTurn") && valve6Anim.GetBool("leftTurn"))
		{
			valveComplete = true;
			if(GameObject.Find("Broken Plant Wall") != null)
			{
				StartCoroutine(DestroyWall());
			}
		}
	}
	IEnumerator DestroyWall()
	{
		steamPipeParticle.Play();
		yield return new WaitForSeconds(5f);
		steamPipeParticle.Stop();
		Destroy(GameObject.Find("Broken Plant Wall"));
	}
}
