using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangerCheckpoint : MonoBehaviour 
{
	private Scene activeScene;
	void Start()
	{
		activeScene = SceneManager.GetActiveScene();
	}
	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.tag == "Player")
		{
			if(activeScene.name == "Chapter 2 - Lava")
			{
				other.gameObject.GetComponent<CharacterControl>().lastCheckpoint = "Chapter 2 - Power Plant";
				SaveSystem.SaveLastCheckpointData(other.gameObject.GetComponent<CharacterControl>());
				SceneManager.LoadScene("Chapter 2 - Power Plant");
			}
			if(activeScene.name == "Fase1parte1")
			{
				other.gameObject.GetComponent<CharacterControl>().lastCheckpoint = "Fase1parte2";
				SaveSystem.SaveLastCheckpointData(other.gameObject.GetComponent<CharacterControl>());
				SceneManager.LoadScene("Fase1parte2");
			}
				if(activeScene.name == "Fase1parte2")
			{
				other.gameObject.GetComponent<CharacterControl>().lastCheckpoint = "Fase1parte3";
				SaveSystem.SaveLastCheckpointData(other.gameObject.GetComponent<CharacterControl>());
				SceneManager.LoadScene("Fase1parte3");
			}
				if(activeScene.name == "Fase1parte3")
			{
				other.gameObject.GetComponent<CharacterControl>().lastCheckpoint = "Fase1parte4";
				SaveSystem.SaveLastCheckpointData(other.gameObject.GetComponent<CharacterControl>());
				SceneManager.LoadScene("Fase1parte4");
			}
			if(activeScene.name == "Fase1parte4")
			{
				other.gameObject.GetComponent<CharacterControl>().lastCheckpoint = "Fase1final";
				SaveSystem.SaveLastCheckpointData(other.gameObject.GetComponent<CharacterControl>());
				SceneManager.LoadScene("Fase1final");
			}
				
				if(activeScene.name == "Fase1final")
			{
				other.gameObject.GetComponent<CharacterControl>().lastCheckpoint = "Fase1final1";
				SaveSystem.SaveLastCheckpointData(other.gameObject.GetComponent<CharacterControl>());
				SceneManager.LoadScene("Fase1final1");
			}
				if(activeScene.name == "Fase1final1")
			{
				other.gameObject.GetComponent<CharacterControl>().lastCheckpoint = "Fase1final2";
				SaveSystem.SaveLastCheckpointData(other.gameObject.GetComponent<CharacterControl>());
				SceneManager.LoadScene("Fase1final2");
			}
				if(activeScene.name == "Fase1final2" && FindObjectOfType<CharacterControl>().HasItem("Time Capsule 1"))
			{
				other.gameObject.GetComponent<CharacterControl>().lastCheckpoint = "Chapter 2 - Lava";
				SaveSystem.SaveLastCheckpointData(other.gameObject.GetComponent<CharacterControl>());
				SceneManager.LoadScene("Chapter 2 - Lava");
			}
			if(activeScene.name == "FaseTutorial")
			{
				other.gameObject.GetComponent<CharacterControl>().lastCheckpoint = "Fase1parte1";
				SaveSystem.SaveLastCheckpointData(other.gameObject.GetComponent<CharacterControl>());
				SceneManager.LoadScene("Fase1parte1");
			}
		}
		
	}

}
