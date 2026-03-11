using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float cardPeekTime = 3f;
    private readonly int fixedScore = 5;
    private Stack<Card> cardStack = new();
    private bool isLevelComplete;

    private int score;
    private int scoreMultiplier;

    private void Start()
    {
        StartCoroutine(StartNewGame());
    }

    private IEnumerator StartNewGame()
    {
        cardStack = new Stack<Card>();
        isLevelComplete = false;
        foreach (var card in FindObjectsOfType<Card>()) card.Initialize(this, CardState.FaceUp);

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