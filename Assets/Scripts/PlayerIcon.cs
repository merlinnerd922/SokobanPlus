using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerIcon : MonoBehaviour
{
    public Image targetImage;
    private PlayerObject _player;

    public Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        targetImage = GetComponent<Image>();
        _player = GetComponentInParent<PlayerObject>();
        camera = FindObjectOfType<Camera>();
        UpdateLocation();
    }

    internal void UpdateLocation()
    {
        targetImage.transform.position = camera.WorldToScreenPoint(_player.transform.position) +
                                         new Vector3(0, 50, 0);
    }
}
