using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeyserWater : MonoBehaviour 
{

	// Use this for initialization
	private SpriteRenderer geyserSprite;
	private Animator geyserAnim;
	private BoxCollider2D geyserBoxCol;
	private BoxCollider2D geyserColTop;
	private float cooldown;
	private float cooldownValue;
	private float upAnimTime;
	private float downAnimTime;
	void Start () {
		geyserBoxCol = GetComponent<BoxCollider2D>();
		geyserSprite = GetComponent<SpriteRenderer>();
		geyserAnim = GetComponent<Animator>();
		geyserColTop = this.transform.GetChild(0).GetComponent<BoxCollider2D>();
		cooldownValue = Random.Range(5f,8f);
		cooldown = cooldownValue;
		geyserAnim.enabled = false;
		geyserSprite.enabled = false;
		geyserColTop.enabled = false;
	}
	
	// Update is called once per frame
	void Update ()
	{	
		this.transform.GetChild(1).position = new Vector2(this.transform.position.x,this.transform.position.y + geyserBoxCol.size.y + 0.15f);
		UpdateGeyserAnimClipTimes();
		cooldown -= Time.deltaTime;
		if(cooldown <= 0)
		{
			StartCoroutine(NoAnim());
		}
	}

	private IEnumerator NoAnim()
	{
		geyserAnim.enabled = true;
		geyserSprite.enabled = true;
		geyserColTop.enabled = true;
		cooldown = cooldownValue + downAnimTime + upAnimTime;
		yield return new WaitForSeconds(downAnimTime + upAnimTime);
		geyserAnim.enabled = false;
		geyserSprite.enabled = false;
		geyserColTop.enabled = false;
	}
	 public void UpdateGeyserAnimClipTimes()
    {
        AnimationClip[] clips = geyserAnim.runtimeAnimatorController.animationClips;
        foreach(AnimationClip clip in clips)
        {
            switch(clip.name)
            {
                case "geyser_water_down":
                    downAnimTime  = clip.length;
                    break;
				case "geyser_water_up":
					upAnimTime = clip.length;
					break;
                
            }
        }
    }
}
