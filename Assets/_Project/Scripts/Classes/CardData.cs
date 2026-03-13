using UnityEngine;
using System;

[Serializable]
public class CardData
{
    [NonSerialized]
    private Sprite sprite;

    [SerializeField] private int uniqueId;
    [SerializeField] private int cardSiblingIndex;

    [SerializeField] private string spriteName;

    [SerializeField] private CardState state;

    public CardData(int uniqueId, Sprite sprite,CardState state)
    {
        this.uniqueId = uniqueId;
        this.sprite = sprite;
        spriteName = sprite.name.Replace("(Clone)","");
        this.state = state;
    }
    
    public void SetState(CardState cardState)
    {
        state = cardState;
    }

    public void SetCardSiblingIndex(int cardSiblingIndex)
    {
        Debug.Log("Setting card sibling index to " + cardSiblingIndex);
        this.cardSiblingIndex = cardSiblingIndex;
    }

    public Sprite GetSprite()
    {
        return sprite;
    }

    public int GetCardSiblingIndex()
    {
        return cardSiblingIndex;
    }
    public CardState GetState()
    {
        return state;
    }
    
    public int GetUniqueId()
    {
        return uniqueId;
    }

}