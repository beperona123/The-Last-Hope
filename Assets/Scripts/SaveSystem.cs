
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem 
{
	public static void SaveLastCheckpointData (CharacterControl characterControl)
	{
		BinaryFormatter formatter = new BinaryFormatter();
		string path = Application.persistentDataPath + "playerCheckpoints.save";
		FileStream stream = new FileStream(path,FileMode.Create);
		PlayerData data = new PlayerData(characterControl);

		formatter.Serialize(stream,data);
		stream.Close();
	}
	public static PlayerData LoadLastCheckpointData()
	{
		string path = Application.persistentDataPath + "playerCheckpoints.save";
		if(File.Exists(path))
		{
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream stream = new FileStream(path,FileMode.Open);

			PlayerData data = formatter.Deserialize(stream) as PlayerData;
			stream.Close();
			return data;
		}
		else
		{
			Debug.LogError("Save file not found in " + path);
			return null;
		}
	}
	public static void ResetCheckpointData ()
	{
		string path = Application.persistentDataPath + "playerCheckpoints.save";
		if(File.Exists(path))
		{
			FileStream stream = new FileStream(path,FileMode.Truncate,FileAccess.Write);
			stream.Close();
		}
	}
	public static bool CheckContinueDisabled ()
	{
		string path = Application.persistentDataPath + "playerCheckpoints.save";
		if(File.Exists(path))
		{
			FileStream stream = new FileStream(path,FileMode.Open);
			if(stream.Length != 0)
			{
				bool data = false;
				stream.Close();
				return data;
				
			}
			else
			{
				bool data = true;
				stream.Close();
				return data;
			}
		}
		else
		{
			bool data = true;
			return data;
		}
	
	}
	public static PlayerData CheckChaptersDisabled()
	{
		string path = Application.persistentDataPath + "playerCheckpoints.save";
		if(File.Exists(path))
		{
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream stream = new FileStream(path,FileMode.Open);
			if(stream.Length != 0)
			{
				PlayerData data = formatter.Deserialize(stream) as PlayerData;
				stream.Close();
				return data;
			}
			else
			{
				stream.Close();
				return null;
			}
		}
		else
		{
			return null;
		}
	}	
}
