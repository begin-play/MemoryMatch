/*
 * Copyright (c) 2026 Sagar Kumar
 * All Rights Reserved.
 */
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    [SerializeField] private GameObject gameBoard;
    [SerializeField] private GameObject referenceCard;
    [SerializeField] private int columnSize = 4;
    [SerializeField] private int rowSize = 4;
    
    private readonly int padding = 10;
    private readonly int spacing = 10;
    
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
        GridLayoutGroup gridLayout = gameBoard.GetComponent<GridLayoutGroup>();

        RectTransform boardRect = gameBoard.GetComponent<RectTransform>();

        gridLayout.padding = new RectOffset(padding, padding, padding, padding);
        gridLayout.spacing = new Vector2(spacing, spacing);

        float totalPaddingW = gridLayout.padding.left + gridLayout.padding.right;
        float totalSpacingW = (columnSize - 1) * gridLayout.spacing.x;
        float cellWidth = (boardRect.rect.width - totalPaddingW - totalSpacingW) / columnSize;

        float totalPaddingH = gridLayout.padding.top + gridLayout.padding.bottom;
        float totalSpacingH = (rowSize - 1) * gridLayout.spacing.y;
        float cellHeight = (boardRect.rect.height - totalPaddingH - totalSpacingH) / rowSize;
        
        gridLayout.cellSize = new Vector2(cellWidth, cellHeight);
    }
}