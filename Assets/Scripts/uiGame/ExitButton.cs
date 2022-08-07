using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButton : SokobanUIButton
{
    // Start is called before the first frame update
    void Start()
    {
        var rectTransform = GetRectTransformAndAnchorToCanvas();
        rectTransform.anchoredPosition = new Vector2(75, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
