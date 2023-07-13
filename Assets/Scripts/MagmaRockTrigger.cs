using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagmaRockTrigger : MonoBehaviour 
{
	private bool thrown;
	public float numberOfRocks = 8;
	public GameObject mediumMagma;
	public GameObject smallMagma;
	private GameObject smallMagmaClone;
	private GameObject mediumMagmaClone;
	private float initialX;
	private float initialY;
	private float throwAngle;
	private float throwForce;
	private Vector3 throwDir;
	void Start()
	{
		thrown = false;
	}
	void OnTriggerEnter2D (Collider2D other)
	{
		if(other.gameObject.tag == "Player" && !thrown)
		{
			thrown = true;
			StartCoroutine(ThrowRocks());
		}
	}
	IEnumerator ThrowRocks()
    {
		for(int i = 0; i < numberOfRocks; i++)
		{	
			throwAngle = Random.Range(225,270);
			throwDir = Quaternion.AngleAxis(throwAngle, Vector3.forward) * Vector3.right;
			yield return new WaitForSeconds(Random.Range(0.5f,0.7f));
			if(Random.value > 0.5)
			{
				throwForce = Random.Range(75,100);
				initialX = mediumMagma.transform.position.x;
				initialY = mediumMagma.transform.position.y;
				mediumMagmaClone = Instantiate(mediumMagma,new Vector3(initialX + (Random.Range(0,16.5f)),initialY,0),Quaternion.identity);
				mediumMagmaClone.GetComponent<Rigidbody2D>().AddForce(throwDir*throwForce);
			}
			else
			{
				throwForce = Random.Range(50,75);
				initialX = smallMagma.transform.position.x;
				initialY = smallMagma.transform.position.y;
				smallMagmaClone = Instantiate(smallMagma,new Vector3(initialX + (Random.Range(0,16.5f)),initialY,0),Quaternion.identity);
				smallMagmaClone.GetComponent<Rigidbody2D>().AddForce(throwDir*throwForce);
			}
		}
    }
}
