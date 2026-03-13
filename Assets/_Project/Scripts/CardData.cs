using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardData 
{
    private int uniqueId;
    private Sprite sprite;
    private int siblingIndex;

    public CardData(int uniqueId, Sprite sprite)
    {
        this.uniqueId = uniqueId;
        this.sprite = sprite;
    }
    public Sprite GetSprite()
    {
        return sprite;
    }
}
