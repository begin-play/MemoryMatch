using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.U2D;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float cardPeekTime = 3f;
    
    [SerializeField] SpriteAtlas spriteAtlas;
    
    private readonly int fixedScore = 5;
    
    private Stack<Card> cardStack = new();
 

    private int score;
    private int scoreMultiplier;
    
    List<CardData> playingCardData = new List<CardData>();
    
    List<Card> playingCards = new List<Card>();
    
    [SerializeField] GridManager gridManager;
    [SerializeField] UIManager uiManager;

    [SerializeField]private Sprite[] spriteArray;
    void Start()
    {
        gridManager.InitializeGrid(ref playingCards);
        if (SaveManager.Instance.IsSaveAvailable())
        {
            playingCardData = SaveManager.Instance.LoadLeveData();
        }
        LoadAtlas();
    }


    public void ResumeGame()
    {
        StartCoroutine(ResumeLastGame());
    }

    public void StartGame()
    {
        StartCoroutine(StartNewGame());
    }

    void LoadAtlas()
    {
        spriteArray = new Sprite[spriteAtlas.spriteCount];
        spriteAtlas.GetSprites(spriteArray);
    }

    private IEnumerator ResumeLastGame()
    {
        cardStack = new Stack<Card>();
        
        for (int i = 0; i < playingCards.Count; i++)
        {
            playingCardData[i].SetCardSiblingIndex(playingCards[i].transform.GetSiblingIndex());
            
            playingCards[i].Initialize(this, playingCardData[i]);
        }

        yield return new WaitForSeconds(cardPeekTime);

        foreach (var card in playingCards) card.Flip();
       
    }
    private IEnumerator StartNewGame()
    {   
        cardStack = new Stack<Card>();
        playingCardData.Clear();
        
      
        spriteArray.Shuffle();

        for (int i = 0; i < playingCards.Count/2; i++)
        {
            CardData cardData = new CardData(i,spriteArray[i],CardState.FaceUp);
            playingCardData.Add(cardData);
            playingCardData.Add(cardData);
       
        }
        playingCardData.Shuffle();
        
        for (int i = 0; i < playingCards.Count; i++)
        {
           
            playingCardData[i].SetCardSiblingIndex(playingCards[i].transform.GetSiblingIndex());
            playingCards[i].ResetCard();
            playingCards[i].Initialize(this, playingCardData[i]);
        }

        yield return new WaitForSeconds(cardPeekTime);

        foreach (var card in playingCards) card.Flip();
    }

    public void ProcessCard(Card incomingCard)
    {
        if (cardStack.Count % 2 == 0)
        {
            cardStack.Push(incomingCard);
        }
        else
        {
            var lastCard = cardStack.Peek();
            if (lastCard.UniqueId == incomingCard.UniqueId)
            {
                cardStack.Push(incomingCard);

                // Hide Both cards
                StartCoroutine(CorrectMatch(lastCard, incomingCard));
                scoreMultiplier++;
            }
            else
            {
                // Mismatch: pop the last card and flip both back
                var poppedCard = cardStack.Pop();
                scoreMultiplier = 0;
                StartCoroutine(IncorrectMatch(poppedCard, incomingCard));
            }

            UpdateScore();
        }
    }

    private void UpdateScore()
    {
        score += fixedScore * scoreMultiplier;
     
    }

    private IEnumerator CorrectMatch(Card lastCard, Card incomingCard)
    {
        yield return new WaitUntil(() => incomingCard.State == CardState.FaceUp);
        lastCard.State = CardState.Matched;
        incomingCard.State = CardState.Matched;
        
        foreach (var cardData in playingCardData)
        {
            if(cardData.GetUniqueId()== incomingCard.UniqueId)
                cardData.SetState(CardState.Matched);
        }
        
        lastCard.HideCard();
        incomingCard.HideCard();


        if (cardStack.Count == playingCards.Count)
        {
           uiManager.LevelCompleteEvent();
        }
    }

    public void ResetBoardCards()
    {
        playingCards.ForEach(x=>x.ResetCardData());

    }
    private IEnumerator IncorrectMatch(Card poppedCard, Card incomingCard)
    {
        yield return new WaitUntil(() => incomingCard.State == CardState.FaceUp);
        poppedCard.Flip();
        incomingCard.Flip();
    }

    public void SaveActiveLevel()
    {
        int matchedCards = playingCardData.Count(x=> x.GetState()==CardState.Matched);
  
        if (matchedCards == 0 || matchedCards == playingCardData.Count)
        {
            SaveManager.Instance.ClearLevelData();
            return;
        }
           
        SaveManager.Instance.SaveLevelData(true,playingCardData);
        
    }
}