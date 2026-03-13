using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private GameObject gameBoard;
    [SerializeField] private GameObject referenceCard;
    [SerializeField] private int columnSize = 4;
    [SerializeField] private int rowSize = 4;
    
    private int gridSize;
    
    [SerializeField, HideInInspector] private bool isGridSizeValid;

    private void OnValidate()
    {
        int rawGridSize = columnSize * rowSize;
        isGridSizeValid = (rawGridSize >= 4 && rawGridSize <= 16 && rawGridSize % 2 == 0);
        
        gridSize = Mathf.Clamp(rawGridSize, 4, 16);
        if (gridSize % 2 != 0)
        {
            gridSize++;
            if (gridSize > 16) gridSize = 16;
        }
    }

    public void InitializeGrid(ref List<Card> playingCards)
    {
       
        for (int i = 0; i < gridSize; i++)
        {
            GameObject cardObject = Instantiate(referenceCard, gameBoard.transform);

            cardObject.transform.SetSiblingIndex(Random.Range(0, gameBoard.transform.childCount));
            playingCards.Add(cardObject.GetComponent<Card>());
        }

        AdjustGridUI();
    }
    private void AdjustGridUI()
    {
       
    }
}