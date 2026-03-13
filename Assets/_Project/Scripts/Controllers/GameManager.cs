/*
 * Copyright (c) 2026 Sagar Kumar
 * All Rights Reserved.
 */
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private int highScore;
    
    List<CardData> playingCardData = new List<CardData>();
    
    List<Card> playingCards = new List<Card>();
    
    [SerializeField] GridManager gridManager;
    [SerializeField] UIManager uiManager;
    [SerializeField] AudioManager audioManager;
    [SerializeField] private GameObject gameBoard;
    [SerializeField]private Sprite[] spriteArray;
    void Start()
    {
        gridManager.InitializeGrid(ref playingCards);
        SaveManager.Instance.LoadScore(ref score, ref highScore, ref scoreMultiplier);
        if (SaveManager.Instance.IsSaveAvailable())
        {
            playingCardData = SaveManager.Instance.LoadLeveData();
        }
        else
        {
            score = 0;
            scoreMultiplier = 0;
        }
        uiManager.UpdateScoreUI(score, highScore, scoreMultiplier);
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
        
        for (int i = 0; i < playingCardData.Count; i++)
        {
            int index = playingCardData[i].GetCardSiblingIndex();
            gameBoard.transform.GetChild(index).GetComponent<Card>().Initialize(this, playingCardData[i]);
           
        }

        yield return new WaitForSeconds(cardPeekTime);

        foreach (var card in playingCards)
        {
            if (card.State == CardState.Matched)
            {
                continue;
            }

            card.Flip();
        }
       
    }
    private IEnumerator StartNewGame()
    {   
        cardStack = new Stack<Card>();
        playingCardData.Clear();
        score = 0;
        scoreMultiplier = 0;
        uiManager.UpdateScoreUI(score, highScore, scoreMultiplier);
        
      
        spriteArray.Shuffle();

        for (int i = 0; i < playingCards.Count/2; i++)
        {
            playingCardData.Add(new CardData(i,spriteArray[i],CardState.FaceUp));
            playingCardData.Add(new CardData(i,spriteArray[i],CardState.FaceUp));
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
        audioManager.PlayCardFlipAudio();
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
        if (score > highScore)
        {
            highScore = score;
        }
        uiManager.UpdateScoreUI(score, highScore, scoreMultiplier);
    }

    private IEnumerator CorrectMatch(Card lastCard, Card incomingCard)
    {
        yield return new WaitUntil(() => incomingCard.State == CardState.FaceUp);
        lastCard.State = CardState.Matched;
        incomingCard.State = CardState.Matched;
        audioManager.PlayCardMatchAudio();
        foreach (var cardData in playingCardData)
        {
            if(cardData.GetUniqueId() == incomingCard.UniqueId)
                cardData.SetState(CardState.Matched);
        }
        
        lastCard.HideCard();
        incomingCard.HideCard();


        if (cardStack.Count == playingCards.Count)
        {
           SaveManager.Instance.UpdateScore(score, highScore, scoreMultiplier);
           SaveManager.Instance.SaveGameData();
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
        audioManager.PlayCardMismatchAudio();
        poppedCard.Flip();
        incomingCard.Flip();
    }

    public void SaveActiveLevel()
    {
        int matchedCards = playingCardData.Count(x=> x.GetState()==CardState.Matched);
  
        if (matchedCards == 0 || matchedCards == playingCardData.Count)
        {
            SaveManager.Instance.ClearLevelData();
            // We still want to save the highscore if it was updated
            SaveManager.Instance.UpdateScore(0, highScore, 0);
            SaveManager.Instance.SaveGameData();
            return;
        }
           
        SaveManager.Instance.UpdateScore(score, highScore, scoreMultiplier);
        SaveManager.Instance.SaveLevelData(true,playingCardData);
        
    }
}