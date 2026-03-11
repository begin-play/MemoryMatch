using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerDownHandler
{
    private readonly float rotationSpeed = 180f;
    private readonly float hideSpeed = 0.5f;

    private GameManager gameManager;

    private bool isFaceUp;

    private CardState state;

    public CardState State
    {
        get => state;
        set => state = value;
    }


    [SerializeField]private int uniqueId;
    public int UniqueId => uniqueId;

    private GameObject faceUp;
    private GameObject faceDown;

    private CanvasGroup canvasGroup;

    void Awake()
    {
        faceUp = transform.Find("FaceUp").gameObject;
        faceDown = transform.Find("FaceDown").gameObject;
        canvasGroup = GetComponent<CanvasGroup>();
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        if (state == CardState.FaceDown && state != CardState.Matched)
        {
            Debug.Log("Card clicked");
            Flip();
            gameManager.ProcessCard(this);
        }
    }


    public void Initialize(GameManager gameManager, CardState state)
    {
        this.gameManager = gameManager;
        this.state = state;
        isFaceUp = (state == CardState.FaceUp);
        if (faceUp != null) faceUp.SetActive(isFaceUp);
        if (faceDown != null) faceDown.SetActive(!isFaceUp);
        float currentRotation = isFaceUp ? 180f : 0f;
        transform.rotation = Quaternion.Euler(0, currentRotation, 0);
    }

    public void Flip()
    {
        state = CardState.Animating;
        StartCoroutine(AnimateCard());
    }

    IEnumerator AnimateCard(bool delayedAnimation = false)
    {
        if (delayedAnimation)
        {
            yield return new WaitForSeconds(1.5f);
        }

        float startRotation = isFaceUp ? 180f : 0f;
        float targetRotation = isFaceUp ? 0f : 180f;
        float currentRotation = startRotation;
        bool isFrontFaceVisible = false;

        while (!Mathf.Approximately(currentRotation, targetRotation))
        {
            currentRotation = Mathf.MoveTowards(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0, currentRotation, 0);

            if (!isFrontFaceVisible)
            {
                float angleProgress = Mathf.Abs((currentRotation - startRotation));
                if (angleProgress >= 90f)
                {
                    isFrontFaceVisible = true;
                    isFaceUp = !isFaceUp;
                    faceUp.SetActive(isFaceUp);
                    faceDown.SetActive(!isFaceUp);
                }
            }

            yield return new WaitForEndOfFrame();
        }

        if (state == CardState.Matched)
        {
            //card already matched nothing to do here; no state change required
        }
        else
        {
            state = isFaceUp ? CardState.FaceUp : CardState.FaceDown;
        }

      
    }


    public void HideCard()
    {
        StartCoroutine(HideCardDelayed());
    }

    IEnumerator HideCardDelayed()
    {
        yield return new WaitForSeconds(0.5f);
        float alpha = canvasGroup.alpha;
        while (!Mathf.Approximately(alpha, 0f))
        {
            alpha = Mathf.MoveTowards(alpha, 0, hideSpeed * Time.deltaTime);
            canvasGroup.alpha = alpha;

            yield return null;
        }
    }
}