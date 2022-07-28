using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartButton : MonoBehaviour
{
    public SokobanGameManager gameManager;

    public void Restart()
    {
        gameManager.Restart();
    }
}
