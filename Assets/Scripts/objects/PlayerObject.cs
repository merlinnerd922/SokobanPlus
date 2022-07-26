using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

public class PlayerObject : MonoBehaviour
{
    private SokobanGameManager _gameManager;

    private const int PLAYER_SPEED = 4;
    
    private Vector2Int? _currentPosition;


    private float PLAYER_ELEVATION = 1;


    public IEnumerator Move(PlayerDirection dir)
    {
        _gameManager._gameState = SokobanGameState.IN_MOTION;
        var thisCurrentPosition = GetCurrentPosition();
        Vector2Int oldPosition = thisCurrentPosition;
        Vector2Int newPosition = thisCurrentPosition + GetIncrementFromDirection(dir);

        float deltaTime = 0;
        while (deltaTime < 1)
        {
            UpdateMovement(deltaTime, oldPosition, newPosition);
            deltaTime += PLAYER_SPEED * Time.deltaTime;
            yield return null;    
        }
        
        SetPlayerPosition(newPosition);
        _gameManager.sokobanBoard.RemoveEmptySlot(newPosition);
        _gameManager.sokobanBoard.AddEmptySlot(oldPosition);
        _gameManager._gameState = SokobanGameState.STATIONARY;
    }

    private Vector2Int GetIncrementFromDirection(PlayerDirection dir)
    {
        return SokobanGameManager.DIRECTION_VECTOR_MAPPING[dir];
    }

    private Vector2Int GetCurrentPosition()
    {
        Debug.Assert(_currentPosition != null, nameof(_currentPosition) + " != null");
        return (Vector2Int) _currentPosition;
    }

    private void UpdateMovement(float deltaTime, Vector2Int oldPosition, Vector2Int newPosition)
    {
        transform.position = Vector3.Lerp(new Vector3(oldPosition.x, PLAYER_ELEVATION, oldPosition.y),
                                            new Vector3(newPosition.x, PLAYER_ELEVATION, newPosition.y),
                                            deltaTime);
    }

    public void Init(SokobanGameManager sokobanGameManager, Vector2Int playerPosition)
    {
        _gameManager = sokobanGameManager;
        SetPlayerPosition(playerPosition);
    }

    private void SetPlayerPosition(Vector2Int playerPosition)
    {
        this._currentPosition = playerPosition;
    }

    public bool CanMove(PlayerDirection directionFromKeyCode)
    {
        // If the player is on the border, they cannot move.
        var newPosition = GetCurrentPosition() + GetIncrementFromDirection(directionFromKeyCode);
        if (!SokobanBoard.PositionIsOnBoard(newPosition))
        {
            return false;
        }
        
        // If the player is being blocked by a block, then prevent movement.
        return !PlayerIsBlockedByBlock(newPosition, directionFromKeyCode);
    }

    private bool PlayerIsBlockedByBlock(Vector2Int newPosition, PlayerDirection directionFromKeyCode)
    {
        // Check if there is actually a block at the targeted position.
        if (!sokobanBoard.PositionHasBlock(newPosition))
        {
            return false;
        }

        // Case 1: The block is being impeded by another block.
        var positionRelativeToNewPositionInDirection = sokobanBoard.GetPositionRelativeToInDirection(newPosition, directionFromKeyCode);
        if (sokobanBoard.PositionHasBlock( positionRelativeToNewPositionInDirection))
        {
            return true;
        }

        // Case 2: The block is being impeded by the edge of the board.
        return !SokobanBoard.PositionIsOnBoard(positionRelativeToNewPositionInDirection);
    }

    public SokobanBoard sokobanBoard => _gameManager.sokobanBoard;
}