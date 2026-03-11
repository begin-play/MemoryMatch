using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
   
    private int score = 0;
    private int fixedScore = 5;
  
    private int scoreMultiplier = 0;

    [SerializeField] private float cardPeekTime = 3f;
    Stack<Card> cardStack = new Stack<Card>();
    bool isLevelComplete = false;

    IEnumerator Start()
    {
        cardStack = new Stack<Card>();
        isLevelComplete = false;
        foreach (Card card in FindObjectsOfType<Card>())
        {
            card.Initialize(this, CardState.FaceUp);
        }

        yield return new WaitForSeconds(cardPeekTime);

        foreach (Card card in FindObjectsOfType<Card>())
        {
            card.Flip();
        }
    }

    public void ProcessCard(Card incomingCard)
    {
        if (cardStack.Count % 2 == 0)
        {
            cardStack.Push(incomingCard);
        }
        else
        {
            Card lastCard = cardStack.Peek();
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
                Card poppedCard = cardStack.Pop();
                scoreMultiplier = 0;
                StartCoroutine(IncorrectMatch(poppedCard, incomingCard));
            }

            UpdateScore();
        }
    }

    void UpdateScore()
    {
        
        score += (fixedScore * scoreMultiplier);
        Debug.Log(score);
    }

    IEnumerator CorrectMatch(Card lastCard, Card incomingCard)
    {
        yield return new WaitUntil(() => incomingCard.State == CardState.FaceUp);
        lastCard.State = CardState.Matched;
        incomingCard.State = CardState.Matched;
        lastCard.HideCard();
        incomingCard.HideCard();


        if (cardStack.Count == FindObjectsOfType<Card>().Length)
        {
            isLevelComplete = true;
        }
    }

    IEnumerator IncorrectMatch(Card poppedCard, Card incomingCard)
    {
        yield return new WaitUntil(() => incomingCard.State == CardState.FaceUp);
        poppedCard.Flip();
        incomingCard.Flip();
    }
}