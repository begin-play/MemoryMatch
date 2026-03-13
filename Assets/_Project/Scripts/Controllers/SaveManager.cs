using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    private SaveData currentSaveData;
    private string saveFilePath;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        saveFilePath = Path.Combine(Application.persistentDataPath, "saveData.json");
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadSaveData();
    }

    private void LoadSaveData()
    {
        currentSaveData = new SaveData();
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            currentSaveData = JsonUtility.FromJson<SaveData>(json);
        }
        // else
        // {
        //     currentSaveData = new SaveData();
        // }
    }

    public void UpdateMusicSettings(float musicVolume, float sfxVolume,bool isMusicOn, bool isSfxOn)
    {
        currentSaveData.musicVolume = musicVolume;
        currentSaveData.sfxVolume = sfxVolume;
        currentSaveData.isMusicOn = isMusicOn;
        currentSaveData.isSfxOn = isSfxOn;
    }

    public void UpdateScore(int score, int highScore, int scoreMultiplier)
    {
        currentSaveData.score = score;
        currentSaveData.highScore = highScore;
        currentSaveData.scoreMultiplier = scoreMultiplier;
    }

    public void SaveLevelData(bool saveAvailable, List<CardData> savedCardData)
    {
        currentSaveData.saveAvailable = saveAvailable;
        currentSaveData.savedCardData = savedCardData;
    }

    public bool IsSaveAvailable()
    {
        return currentSaveData.saveAvailable;
    }
    public void ClearLevelData()
    {
        currentSaveData.saveAvailable = false;
        currentSaveData = new SaveData();
    }

    public List<CardData> LoadLeveData()
    {
        return currentSaveData.savedCardData;
    }
    public void SaveGameData()
    {
        string json = JsonUtility.ToJson(currentSaveData);
        File.WriteAllText(saveFilePath, json);
    }
}
