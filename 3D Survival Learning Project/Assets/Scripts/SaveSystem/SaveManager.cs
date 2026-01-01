using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class SaveManager : MonoBehaviour
{
    public PlayerInfo playerInfo;
    public event Action OnSaveRequested;
    public event Action OnLoadCompleted;
    public static SaveManager instance;

    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.F5))
        {
            SaveAll();
        }

        if (Input.GetKeyUp(KeyCode.F6))
        {
            LoadAll();
        }
    }

    public void SaveAll()
    {
        OnSaveRequested?.Invoke();
        Save("playerInfo", playerInfo);
        Debug.Log("Saving the progress");
    }

    public void LoadAll()
    {
        playerInfo = Load<PlayerInfo>("playerInfo");
        OnLoadCompleted?.Invoke();
        Debug.Log("Loading the progress");
    }

    public void Save<T>(string fileName, T data)
    {
        string fullPath = Application.persistentDataPath + $"/{fileName}.json";
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(fullPath, json);
    }

    public T Load<T>(string fileName)
    {
        string fullPath = Application.persistentDataPath + $"/{fileName}.json";
        if (File.Exists(fullPath))
        {
            string json = File.ReadAllText(fullPath);
            return JsonUtility.FromJson<T>(json);
        }
        else
        {
            return default;
        }
    }
}

[Serializable]
public class PlayerInfo
{
    public Vector3 position;
}