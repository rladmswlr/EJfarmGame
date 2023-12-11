using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
	static readonly string FILEPATH = Application.persistentDataPath + "/Save.json";

    public static void Save(GameSaveState save)
	{
		
		string json = JsonUtility.ToJson(save);
		File.WriteAllText(FILEPATH, json);
	}

	public static GameSaveState Load()
	{
		GameSaveState loadSave = null;

		if(File.Exists(FILEPATH))
		{
			string json = File.ReadAllText(FILEPATH);
			loadSave = JsonUtility.FromJson<GameSaveState>(json);
		}

		return loadSave;
	}

	public static bool HasSave()
	{
		return File.Exists(FILEPATH);
	}
}
