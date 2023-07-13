using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaDrop : MonoBehaviour {

	SpriteRenderer dropSprite;
	Animator dropAnim;
	BoxCollider2D dropCol;
	float dropCooldown;
	float dropCooldownValue;
	float dropAnimTime;
	void Start () 
	{
		dropCol = GetComponent<BoxCollider2D>();
		dropSprite = GetComponent<SpriteRenderer>();
		dropAnim = GetComponent<Animator>();
		dropCooldownValue = Random.Range(2f,5f);
		dropCooldown = dropCooldownValue;
		dropAnim.enabled = false;
		dropSprite.enabled = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		UpdateDropAnimClipTimes();
		dropCooldown -= Time.deltaTime;
		if(dropCooldown <= 0)
		{
			StartCoroutine(NoDropAnim());
		}
		
	}
	public void UpdateDropAnimClipTimes()
    {
        AnimationClip[] clips = dropAnim.runtimeAnimatorController.animationClips;
        foreach(AnimationClip clip in clips)
        {
            switch(clip.name)
            {
                case "drop":
                    dropAnimTime = clip.length;
                    break;
                
            }
        }
    }
		private IEnumerator NoDropAnim()
	{
		dropAnim.enabled = true;
		dropSprite.enabled = true;
		dropCol.enabled = true;
		dropCooldown = dropCooldownValue + dropAnimTime;
		yield return new WaitForSeconds(dropAnimTime);
		dropAnim.enabled = false;
		dropSprite.enabled = false;
		dropCol.enabled = false;
	}
}
