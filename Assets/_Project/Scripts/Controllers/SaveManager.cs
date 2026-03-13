using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

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
        
        saveFilePath = Path.Combine(Application.persistentDataPath, "savedata.json");
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
    }
    
    
    #region Music Settings

    public void SetMusicSettings(float musicVolume, float sfxVolume, bool isMusicOn, bool isSfxOn)
    {
        currentSaveData.musicVolume = musicVolume;
        currentSaveData.sfxVolume = sfxVolume;
        currentSaveData.isMusicOn = isMusicOn;
        currentSaveData.isSfxOn = isSfxOn;
        
        SaveGameData();
    }
    public void LoadMusicSettings(ref float musicVolume,ref float sfxVolume,ref bool isMusicOn,ref bool isSfxOn)
    {
        musicVolume = currentSaveData.musicVolume;
        sfxVolume = currentSaveData.sfxVolume;
        isMusicOn = currentSaveData.isMusicOn;
        isSfxOn = currentSaveData.isSfxOn;
    }
    public void LoadMusicSettings(ref Slider musicVolume,ref Slider sfxVolume,ref Toggle isMusicOn,ref Toggle isSfxOn)
    {
        musicVolume.value = currentSaveData.musicVolume;
        sfxVolume.value = currentSaveData.sfxVolume;
        isMusicOn.isOn = currentSaveData.isMusicOn;
        isSfxOn.isOn = currentSaveData.isSfxOn;
    }
    
    #endregion
    
    #region Score Board Data
    public void UpdateScore(int score, int highScore, int scoreMultiplier)
    {
        currentSaveData.score = score;
        currentSaveData.highScore = highScore;
        currentSaveData.scoreMultiplier = scoreMultiplier;
    }

    public void LoadScore(ref int score,ref int highScore,ref int scoreMultiplier)
    {
        score = currentSaveData.score;
        highScore = currentSaveData.highScore;
        scoreMultiplier = currentSaveData.scoreMultiplier;
    }
    #endregion
    
    public void SaveLevelData(bool saveAvailable, List<CardData> savedCardData)
    {
        currentSaveData.saveAvailable = saveAvailable;
        currentSaveData.savedCardData = savedCardData;
        SaveGameData();
    }

    public bool IsSaveAvailable()
    {
        return currentSaveData.saveAvailable;
    }
    public void ClearLevelData()
    {
        currentSaveData.saveAvailable = false;
        currentSaveData.savedCardData = new List<CardData>();
        SaveGameData();
    }

    public List<CardData> LoadLeveData()
    {
        return currentSaveData.savedCardData;
    }
    public void SaveGameData()
    {
        string json = JsonUtility.ToJson(currentSaveData,true);
        File.WriteAllText(saveFilePath, json);
    }
}
