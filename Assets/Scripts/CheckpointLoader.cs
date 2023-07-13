using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointLoader : MonoBehaviour 
{

	public void CheckpointLoad()
	{
		PlayerData data = SaveSystem.LoadLastCheckpointData();
		Debug.Log(data.lastCheckpoint);
		SceneManager.LoadScene(data.lastCheckpoint);
	}

	public void Chapter1Load()
	{
		SaveSystem.ResetCheckpointData();
		SceneManager.LoadScene("FaseTutorial");
	}
	public void Chapter2Load()
	{
		SaveSystem.ResetCheckpointData();
		SceneManager.LoadScene("Chapter 2 - Lava");
	}
	public void NewGame()
	{
		SceneManager.LoadScene("FaseTutorial");
		SaveSystem.ResetCheckpointData();
	}
}
