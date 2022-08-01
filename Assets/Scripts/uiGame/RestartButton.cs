using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartButton : MonoBehaviour
{
    public SokobanGameManager gameManager;

    public void Start()
    {
        var rectTransform = GetComponent<RectTransform>();
        var anchorsVector = new Vector2(0,0f);
        rectTransform.pivot = anchorsVector;
        rectTransform.anchorMin = anchorsVector;
        rectTransform.anchorMax = anchorsVector;
        rectTransform.anchoredPosition = new Vector2(0, 0);
    }

    public void Restart()
    {
        gameManager.Restart();
    }
}
