using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TheEnd : MonoBehaviour {

	public Dialogue dialogueBefore;
	private GameObject eIcon;
	private Animator videoAnim;
	private AudioSource audioSource;
	private VideoPlayer endVideo;
	private GameObject videoCanvas;
	private Animator eIconAnim;
	public bool videoPlayed;
	private bool isInside;
	private bool isInFirst;
	private bool isInSecond;
		void Start()
	{
		videoCanvas =  GameObject.Find("Video UI");
		videoAnim = videoCanvas.transform.GetChild(0).GetComponent<Animator>();
		endVideo = videoCanvas.transform.GetChild(0).GetChild(0).GetComponent<VideoPlayer>();
		audioSource = videoCanvas.transform.GetChild(0).GetChild(0).GetComponent<AudioSource>();
		eIcon = GameObject.Find("Player").transform.GetChild(1).gameObject;
		eIconAnim = eIcon.GetComponent<Animator>();
	}
	public void TriggerDialogueBefore ()
	{
		FindObjectOfType<DialogueManager>().StartDialogue(dialogueBefore);
	}
	public IEnumerator TriggerVideo()
	{
		audioSource.Play();
		FindObjectOfType<Level1Theme>().audioSource.Stop();
		FindObjectOfType<CharacterControl>().talking = true;
		videoAnim.SetBool("videoUp", true);
		endVideo.Play();
		yield return new WaitForSeconds((float)endVideo.clip.length);
		endVideo.Stop();
		videoAnim.SetBool("videoUp", false);
		FindObjectOfType<CharacterControl>().talking = false;
		videoPlayed = true;
	}
	void Update () 
	{
		if(audioSource.isPlaying)
		{
			Debug.Log("tocando");
		}
		endVideo.SetDirectAudioVolume(0,PlayerPrefs.GetFloat("sTVolume",1));
		if(isInFirst && !FindObjectOfType<DialogueManager>().dialoguing)
		{
			StartCoroutine(TriggerVideo());
		}
		if(videoPlayed == true)
		{

			SceneManager.LoadScene("TelaInicio");
		}
		if(isInside)
		{
			eIcon.SetActive(true);
			if(Input.GetKeyDown(KeyCode.E))
			{
				eIconAnim.SetTrigger("triggered");
				isInFirst = true;
				TriggerDialogueBefore();
				this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
			}
		}
	}
	void OnTriggerEnter2D(Collider2D other)
	{
		 if(other.gameObject.tag =="Player")
        {
            isInside = true;
        }
	}
	void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.tag =="Player")
		{
			isInside = false;
			eIcon.SetActive(false);
		}
	}
}
