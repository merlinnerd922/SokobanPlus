using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using static DefaultNamespace.Extend;

public class SokobanGameManager : MonoBehaviour
{
    public SokobanBoard sokobanBoard;

    internal SokobanGameState gameState;

    private PlayerObject player => sokobanBoard.thisPlayer;

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
    
    public static readonly Dictionary<PlayerDirection, Vector2Int> DIRECTION_VECTOR_MAPPING = new()
    {
        {PlayerDirection.LEFT, Vector2Int.left},
        {PlayerDirection.RIGHT, Vector2Int.right},
        {PlayerDirection.FORWARD, Vector2Int.up},
        {PlayerDirection.BACKWARD, Vector2Int.down}
    };
}

public enum SokobanGameState
{
    STATIONARY = 0,
    IN_MOTION = 1
}