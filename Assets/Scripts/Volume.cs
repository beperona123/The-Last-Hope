using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volume : MonoBehaviour 
{
	public static float fXVolume;
	public static float sTVolume;

	public static void OnFXVolumeChange(float effectVolume)
	{
		fXVolume = effectVolume;
		PlayerPrefs.SetFloat("fXVolume",fXVolume);
	}
	public static void OnSTVolumeChange(float SoundtrackVolume)
	{
		sTVolume = SoundtrackVolume;
		PlayerPrefs.SetFloat("sTVolume",sTVolume);
	}
	
}
