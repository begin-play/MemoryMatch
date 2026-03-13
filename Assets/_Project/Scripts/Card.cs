using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerDownHandler
{
    private readonly float rotationSpeed = 360f;
    private readonly float hideSpeed = 5f;

    private GameManager gameManager;

    private bool isFaceUp;

    private CardState state;

    [SerializeField] private int cardSiblingIndex;
    public CardState State
    {
        get => state;
        set => state = value;
    }


    [SerializeField] private int uniqueId;
    public int UniqueId => uniqueId;

    private GameObject faceUp;
    private GameObject faceDown;
    private Image imageContent;


    private CanvasGroup canvasGroup;

    void Awake()
    {
        faceUp = transform.Find("FaceUp").gameObject;
        faceDown = transform.Find("FaceDown").gameObject;
        imageContent = transform.Find("FaceUp/Content").GetComponent<Image>();
        canvasGroup = GetComponent<CanvasGroup>();
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        if (state == CardState.FaceDown && state != CardState.Matched)
        {
            Flip();
            gameManager.ProcessCard(this);
        }
    }


    public void Initialize(GameManager gameManager, CardData cardData)
    {
        this.gameManager = gameManager;
        this.state = cardData.GetState();
        this.uniqueId = cardData.GetUniqueId();
        imageContent.sprite = cardData.GetSprite();
        cardSiblingIndex = cardData.GetCardSiblingIndex();

        isFaceUp = (state == CardState.FaceUp);
        if (faceUp != null) faceUp.SetActive(isFaceUp);
        if (faceDown != null) faceDown.SetActive(!isFaceUp);
        float currentRotation = isFaceUp ? 180f : 0f;
        transform.rotation = Quaternion.Euler(0, currentRotation, 0);

        if (cardData.GetState() == CardState.Matched)
        {
            canvasGroup.alpha = 0f;
            transform.rotation = Quaternion.Euler(0, 180f, 0);
        }
    }

    public void Flip()
    {
        state = CardState.Animating;
        if(gameObject.activeInHierarchy)
            StartCoroutine(nameof(AnimateCard));
    }

    IEnumerator AnimateCard()
    {
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

    public void ResetCardData()
    {
        StopRunningCoruotines();
    }

    public void ResetCard()
    {
        canvasGroup.alpha = 1f;
        transform.rotation = Quaternion.Euler(0f, 180f, 0f);
    }

    void StopRunningCoruotines()
    {
        StopCoroutine(nameof(HideCardDelayed));
        StopCoroutine(nameof(AnimateCard));
    }

    public void HideCard()
    {
        StartCoroutine(nameof(HideCardDelayed));
    }

    IEnumerator HideCardDelayed()
    {
        yield return new WaitForSeconds(0.5f);
        float alpha = canvasGroup.alpha;
        while (!Mathf.Approximately(alpha, 0f))
        {
            alpha = Mathf.MoveTowards(alpha, 0, hideSpeed * Time.deltaTime);
            alpha = Mathf.Clamp(alpha, 0f, 1f);
            canvasGroup.alpha = alpha;

            yield return null;
        }
    }
}