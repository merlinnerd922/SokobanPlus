using System.Collections;
using UnityEngine;

public class PlayerObject : SokobanMovable
{

    private const int PLAYER_SPEED = 5;



    private float PLAYER_ELEVATION = 1;


    public IEnumerator Move(PlayerDirection dir)
    {
        _gameManager._gameState = SokobanGameState.IN_MOTION;
        var thisCurrentPosition = GetCurrentPosition();
        Vector2Int playerOldPosition = thisCurrentPosition;
        Vector2Int playerNewPosition = thisCurrentPosition + SokobanBoard.GetVectorInDirection(dir);

        // If the player is moving in the direction of a block, then push that block as well.
        SokobanBlock blockToMove = null;
        bool blockToMoveIsNonNull = false;
        Vector2Int? blockOldPosition = null;
        Vector2Int? blockNewPosition = null;

        if (PositionHasBlock(playerNewPosition))
        {
            blockToMoveIsNonNull = true;
            blockToMove = GetBlock(playerNewPosition);
            blockOldPosition = playerNewPosition;
            blockNewPosition = blockOldPosition + SokobanBoard.GetVectorInDirection(dir);
        }

        float deltaTime = 0;

        while (deltaTime < 1)
        {
            // TODO
            UpdateMovement(deltaTime, playerOldPosition, playerNewPosition, PLAYER_ELEVATION);
            if (blockToMoveIsNonNull)
            {
                blockToMove.UpdateMovement(deltaTime, (Vector2Int) blockOldPosition, (Vector2Int) blockNewPosition,
                    sokobanBlockElevation);
            }

            deltaTime += PLAYER_SPEED * Time.deltaTime;
            yield return null;
        }

        SetSokobanPosition(playerNewPosition);
        
        _gameManager.sokobanBoard.AddEmptySlot(playerOldPosition);
        
        // TODO
        if (blockToMoveIsNonNull)
        {
            blockToMove.SetSokobanPosition((Vector2Int) blockNewPosition);
        }
        else
        {
            _gameManager.sokobanBoard.RemoveEmptySlot(playerNewPosition);
        }

        _gameManager._gameState = SokobanGameState.STATIONARY;
    }


    private float sokobanBlockElevation => SokobanBoard.BLOCK_ELEVATION;


    private SokobanBlock GetBlock(Vector2Int playerNewPosition)
    {
        return sokobanBoard.GetBlock(playerNewPosition);
    }

    private bool PositionHasBlock(Vector2Int blockPosition)
    {
        return sokobanBoard.PositionHasBlock(blockPosition);
    }

    private Vector2Int GetCurrentPosition()
    {
        Debug.Assert(_currentGridPosition != null, nameof(_currentGridPosition) + " != null");
        return (Vector2Int) _currentGridPosition;
    }

    public bool CanMove(PlayerDirection directionFromKeyCode)
    {
        // If the player is on the border, they cannot move.
        var newPosition = GetCurrentPosition() + SokobanBoard.GetVectorInDirection(directionFromKeyCode);
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
        var positionRelativeToNewPositionInDirection =
            sokobanBoard.GetPositionRelativeToInDirection(newPosition, directionFromKeyCode);
        if (sokobanBoard.PositionHasBlock(positionRelativeToNewPositionInDirection))
        {
            return true;
        }

        // Case 2: The block is being impeded by the edge of the board.
        return !SokobanBoard.PositionIsOnBoard(positionRelativeToNewPositionInDirection);
    }

}