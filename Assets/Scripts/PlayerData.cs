using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class PlayerData
{
	public string lastCheckpoint;
	public PlayerData(CharacterControl characterControl)
	{
		lastCheckpoint = characterControl.lastCheckpoint;
	}
	
}
