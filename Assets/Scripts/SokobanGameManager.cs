using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DefaultNamespace.Extend;

public class SokobanGameManager : MonoBehaviour
{
    public SokobanBoard sokobanBoard;

    internal SokobanGameState _gameState;

    public PlayerObject player => sokobanBoard.thisPlayer;

    private readonly IEnumerable<KeyCode> _directionKeyCodes =
        HashSetOf(KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.UpArrow, KeyCode.DownArrow);

    private readonly Dictionary<KeyCode, PlayerDirection> _keyCodeDirectionMapping =
        new Dictionary<KeyCode, PlayerDirection>()
        {
            {KeyCode.LeftArrow, PlayerDirection.LEFT},
            {KeyCode.RightArrow, PlayerDirection.RIGHT},
            {KeyCode.UpArrow, PlayerDirection.FORWARD},
            {KeyCode.DownArrow, PlayerDirection.BACKWARD}
        };


    public void Start()
    {
        sokobanBoard.Initialize(this);
        StartReceivingInput();
    }

    private void StartReceivingInput()
    {
        StartCoroutine(GetStartReceivingInputCoroutine());
    }


    private IEnumerator GetStartReceivingInputCoroutine()
    {
        while (true)
        {
            if (_gameState == SokobanGameState.STATIONARY)
            {
                KeyCode? pressedKeyCode = GetPressedDirectionCode();
                if (pressedKeyCode != null)
                {
                    StartCoroutine(player.Move(GetDirectionFromKeyCode((KeyCode) pressedKeyCode)));
                }
            }

            yield return null;
        }
    }

    private PlayerDirection GetDirectionFromKeyCode(KeyCode pressedKeyCode)
    {
        return _keyCodeDirectionMapping[(KeyCode) pressedKeyCode];
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
}

public enum SokobanGameState
{
    STATIONARY = 0,
    IN_MOTION = 1
}