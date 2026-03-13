using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent( typeof( CanvasScaler ))]
public class CanvasManager : MonoBehaviour
{
    private CanvasScaler canvasScaler;
    private const float ReferenceAspectRatio = 1920f / 1080f;

    private int lastWidth;
    private int lastHeight;

  bool initialized = false;

    void Start()
    {
        canvasScaler = GetComponent<CanvasScaler>();
        initialized = true;
        UpdateCanvasScaler();
        
    }

    void OnRectTransformDimensionsChange()
    {
        if (Screen.width != lastWidth || Screen.height != lastHeight)
        {
            UpdateCanvasScaler();
        }
    }

    private void UpdateCanvasScaler()
    {
        if(!initialized)
            return;
        
        lastWidth = Screen.width;
        lastHeight = Screen.height;

        float currentAspectRatio = (float)Screen.width / Screen.height;

        if (currentAspectRatio < ReferenceAspectRatio)
        {
            canvasScaler.matchWidthOrHeight = 0f;
        }
        else
        {
            canvasScaler.matchWidthOrHeight = 1f;
        }
    }
}
