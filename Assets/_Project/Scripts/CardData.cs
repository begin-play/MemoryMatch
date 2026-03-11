using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardData 
{
    public int uniqueId;
    private Sprite sprite;
    

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
