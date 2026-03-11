using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float cardPeekTime = 3f;
    [SerializeField]SpriteAtlas spriteAtlas;
    private readonly int fixedScore = 5;
    private Stack<Card> cardStack = new();
    private bool isLevelComplete;

    private int score;
    private int scoreMultiplier;
    List<CardData> allCardData = new List<CardData>();
    List<CardData> playingCardData = new List<CardData>();
    [SerializeField] private GameObject gameBoard;
    [SerializeField]private GameObject referenceCard;
    [SerializeField] private int gridSize=6;
    public void StartGame()
    {
       
        StartCoroutine(StartNewGame());
    }

    void LoadAtlas()
    {
        Sprite[] spriteArray= new Sprite[spriteAtlas.spriteCount];
        spriteAtlas.GetSprites(spriteArray);
        for (int i = 0; i < spriteAtlas.spriteCount; i++)
        {
            CardData spriteData = new CardData(i, spriteArray[i]);
            allCardData.Add(spriteData);
        }
        
        allCardData.Shuffle();
        playingCardData.Clear();
        allCardData.Take(gridSize/2).ToList().ForEach(x => playingCardData.Add(x));
    }

    private IEnumerator StartNewGame()
    {
        LoadAtlas();
        cardStack = new Stack<Card>();
        isLevelComplete = false;
        
        for (int i= 0; i < playingCardData.Count * 2 ; i++)
        {
            GameObject cardObject = Instantiate(referenceCard, gameBoard.transform);
          
            cardObject.transform.SetSiblingIndex(Random.Range(0, gameBoard.transform.childCount));
        }
       
        List<Card> cards = FindObjectsOfType<Card>().ToList();
        for (int i = 0; i < playingCardData.Count; i++)
        {
            cards[i].Initialize(this, CardState.FaceUp,i,playingCardData[i].GetSprite());
            cards[i+playingCardData.Count].Initialize(this, CardState.FaceUp,i,playingCardData[i].GetSprite());
            Debug.Log(cards[i].UniqueId);
        }

       

        yield return new WaitForSeconds(cardPeekTime);

        foreach (var card in FindObjectsOfType<Card>()) card.Flip();
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
        Debug.Log(score);
    }

    private IEnumerator CorrectMatch(Card lastCard, Card incomingCard)
    {
        yield return new WaitUntil(() => incomingCard.State == CardState.FaceUp);
        lastCard.State = CardState.Matched;
        incomingCard.State = CardState.Matched;
        lastCard.HideCard();
        incomingCard.HideCard();


        if (cardStack.Count == FindObjectsOfType<Card>().Length) isLevelComplete = true;
    }

    private IEnumerator IncorrectMatch(Card poppedCard, Card incomingCard)
    {
        yield return new WaitUntil(() => incomingCard.State == CardState.FaceUp);
        poppedCard.Flip();
        incomingCard.Flip();
    }
}