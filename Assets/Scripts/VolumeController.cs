using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour {

	private Slider fX;
	private Slider sT;
	
	void Start () 
	{
		fX =this.gameObject.transform.GetChild(0).GetComponent<Slider>();
		sT =this.gameObject.transform.GetChild(1).GetComponent<Slider>();
		fX.value = PlayerPrefs.GetFloat("fXVolume", 1);
		sT.value = PlayerPrefs.GetFloat("sTVolume", 1);
	}
	
	public void OnEffectsVolumeChange()
	{
		Volume.OnFXVolumeChange(fX.value);
	}
	public void OnSoundtrackVolumeChange()
	{
		Volume.OnSTVolumeChange(sT.value);
	}

}
