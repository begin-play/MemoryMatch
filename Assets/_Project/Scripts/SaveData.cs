using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    private bool saveAvailable;
    
    private bool isMusicOn;
    private bool isSfxOn;
    
    private int highScore;
    private int score;
    private int scoreMultiplier;
    
    private float musicVolume;
    private float sfxVolume;
    
    List<CardData> savedCardData;
    
}
