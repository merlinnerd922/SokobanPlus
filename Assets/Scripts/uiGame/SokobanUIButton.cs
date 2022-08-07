using UnityEngine;

public class SokobanUIButton : MonoBehaviour
{
    protected RectTransform GetRectTransformAndAnchorToCanvas()
    {
        var rectTransform = GetComponent<RectTransform>();
        var anchorsVector = new Vector2(0, 0f);
        rectTransform.pivot = anchorsVector;
        rectTransform.anchorMin = anchorsVector;
        rectTransform.anchorMax = anchorsVector;
        return rectTransform;
    }
}