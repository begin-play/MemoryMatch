/*
 * Copyright (c) 2026 Sagar Kumar
 * All Rights Reserved.
 */
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public bool saveAvailable = false;
    
    public bool isMusicOn = true;
    public bool isSfxOn = true;
    
    public float musicVolume = 0.5f;
    public float sfxVolume = 0.5f;
    
    public int highScore = 0;
    public int score = 0;
    public int scoreMultiplier = 1;
    
    public List<CardData> savedCardData = new List<CardData>();
}
