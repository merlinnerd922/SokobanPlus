using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using static DefaultNamespace.Extend;

public class SokobanGameManager : MonoBehaviour
{
    public SokobanBoard sokobanBoard;

    internal SokobanGameState gameState;

    public SokobanBoard sokobanBoardPrefab;

    private PlayerObject player => sokobanBoard.boardInfo.thisPlayer;

    private readonly IEnumerable<KeyCode> _directionKeyCodes =
        HashSetOf(KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.UpArrow, KeyCode.DownArrow);

    private readonly Dictionary<KeyCode, PlayerDirection> _keyCodeDirectionMapping =
        new()
        {
            {KeyCode.LeftArrow, PlayerDirection.LEFT},
            {KeyCode.RightArrow, PlayerDirection.RIGHT},
            {KeyCode.UpArrow, PlayerDirection.FORWARD},
            {KeyCode.DownArrow, PlayerDirection.BACKWARD}
        };


    public void Start()
    {
        StartNewGame();
    }

    private void StartNewGame()
    {
        sokobanBoard.Init(this);
        StartReceivingInput();
    }

    private void StartReceivingInput()
    {
        StartCoroutine(GetStartReceivingInputCoroutine());
    }


    [SuppressMessage("ReSharper", "IteratorNeverReturns")]
    private IEnumerator GetStartReceivingInputCoroutine()
    {
        gameState = SokobanGameState.STATIONARY;
        while (true)
        {
            if (gameState == SokobanGameState.STATIONARY)
            {
                ProcessStationaryState();
            }

            yield return null;
        }
    }

    private void ProcessStationaryState()
    {
        KeyCode? pressedKeyCode = GetPressedDirectionCode();
        if (pressedKeyCode == null)
        {
            return;
        }
        
        PlayerDirection directionFromKeyCode = GetDirectionFromKeyCode((KeyCode) pressedKeyCode);
        if (!player.CanMove(directionFromKeyCode))
        {
            return;
        }

        StartCoroutine(player.Move(directionFromKeyCode));
    }

    private PlayerDirection GetDirectionFromKeyCode(KeyCode pressedKeyCode)
    {
        return _keyCodeDirectionMapping[pressedKeyCode];
    }

    private KeyCode? GetPressedDirectionCode()
    {
        foreach (KeyCode keyCode in _directionKeyCodes)
        {
            if (Input.GetKey(keyCode))
            {
                return keyCode;
            }
        }
        return null;
    }
    
    public static readonly Dictionary<PlayerDirection, SVector2Int> DIRECTION_VECTOR_MAPPING = new()
    {
        {PlayerDirection.LEFT, SVector2Int.left},
        {PlayerDirection.RIGHT, SVector2Int.right},
        {PlayerDirection.FORWARD, SVector2Int.up},
        {PlayerDirection.BACKWARD, SVector2Int.down}
    };

    public void Restart()
    {
        // Destroy the previous board and re-init it anew.
        Destroy(sokobanBoard.gameObject);
        sokobanBoard = Instantiate(sokobanBoardPrefab);
        StartNewGame();
    }

    public void DeclareVictory()
    {
        Restart();
    }
}

public enum SokobanGameState
{
    STATIONARY = 0,
    IN_MOTION = 1
}