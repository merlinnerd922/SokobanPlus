using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadButton : SokobanUIButton
{
    // Start is called before the first frame update
    public void Start()
    {
        var rectTransform = GetRectTransformAndAnchorToCanvas();
        rectTransform.anchoredPosition = new Vector2(0, 225);
    }

}
