using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerIcon : MonoBehaviour
{
    public Image targetImage;
    private PlayerObject _player;

    [FormerlySerializedAs("camera")] public Camera playerCamera;

    // Start is called before the first frame update
    void Start()
    {
        targetImage = GetComponent<Image>();
        _player = GetComponentInParent<PlayerObject>();
        playerCamera = FindObjectOfType<Camera>();
        UpdateArrowSpriteLocation();
    }

    internal void UpdateArrowSpriteLocation()
    {
        targetImage.transform.position = playerCamera.WorldToScreenPoint(_player.transform.position) +
                                         new Vector3(0, 50, 0);
    }
}
