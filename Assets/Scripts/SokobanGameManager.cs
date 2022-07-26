using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SokobanGameManager : MonoBehaviour
{
    public SokobanBoard sokobanBoard;

    private SokobanGameState _gameState;
    
    public void Start()
    {
        sokobanBoard.Initialize();
        StartReceiveInput();
    }

    private void StartReceiveInput()
    {
        StartCoroutine(StartReceivingInput());
    }

    private IEnumerator StartReceivingInput()
    {
        while (true)
        {
            yield return null;
        }
    }
}

public enum SokobanGameState
{
    STATIONARY = 0,
    IN_MOTION = 1
}

