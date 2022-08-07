using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartButton : SokobanUIButton
{
    public SokobanGameManager gameManager;

    public void Start()
    {
        var rectTransform = GetRectTransformAndAnchorToCanvas();
        rectTransform.anchoredPosition = new Vector2(0, 0);
    }

    public void Restart()
    {
        gameManager.Restart();
    }
}
