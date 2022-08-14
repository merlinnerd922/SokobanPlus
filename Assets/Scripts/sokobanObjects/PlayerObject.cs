using System.Collections;
using UnityEngine;

public class PlayerObject : SokobanMovable
{
    private const int PLAYER_SPEED = 5;


    public PlayerIcon playerIcon;

    private float _playerElevation = 1;

    public override void SetSokobanPosition(SVector2Int positionToSet, SVector2Int previousPosition,
        MovementSetType setType)
    {
        base.SetSokobanPosition(positionToSet, previousPosition, setType);
        sokobanBoard.boardInfo._boardData.playerPosition = positionToSet;
    }

    public IEnumerator Move(PlayerDirection dir)
    {
        gameManager.gameState = SokobanGameState.IN_MOTION;
        var thisCurrentPosition = GetCurrentPosition();
        SVector2Int playerOldPosition = thisCurrentPosition;
        SVector2Int playerNewPosition = thisCurrentPosition + SokobanBoardInfo.GetVectorInDirection(dir);

        // If the player is moving in the direction of a block, then push that block as well.
        SokobanBlock blockToMove = null;
        bool blockToMoveIsNonNull = false;
        SVector2Int blockOldPosition = null;
        SVector2Int blockNewPosition = null;

        if (PositionHasBlock(playerNewPosition))
        {
            blockToMoveIsNonNull = true;
            blockToMove = GetBlock(playerNewPosition);
            blockOldPosition = playerNewPosition;
            blockNewPosition = blockOldPosition + SokobanBoardInfo.GetVectorInDirection(dir);
        }

        float deltaTime = 0;

        while (deltaTime < 1)
        {
            UpdateMovement(deltaTime, playerOldPosition, playerNewPosition, _playerElevation);

            // If the player's pushing a block, then push the block as well.
            if (blockToMoveIsNonNull)
            {
                blockToMove.UpdateMovement(deltaTime, (SVector2Int) blockOldPosition, (SVector2Int) blockNewPosition,
                    sokobanBlockElevation);
            }

            deltaTime += PLAYER_SPEED * Time.deltaTime;
            yield return null;
        }

        SetSokobanPosition(playerNewPosition, playerOldPosition, MovementSetType.MOVE);

        gameManager.sokobanBoard.boardInfo.AddEmptySlot(playerOldPosition);

        // If a block was pushed, set the block's new position.
        if (blockToMoveIsNonNull)
        {
            blockToMove.SetSokobanPosition((SVector2Int) blockNewPosition, blockOldPosition, MovementSetType.MOVE);
        }

        // Otherwise, only remove info on the player's new position.
        else
        {
            gameManager.sokobanBoard.boardInfo.RemoveEmptySlot(playerNewPosition);
        }


        gameManager.gameState = SokobanGameState.STATIONARY;
    }

    protected internal override void UpdateMovement(float deltaTime, SVector2Int oldPosition, SVector2Int newPosition,
        float
            objElevation)
    {
        base.UpdateMovement(deltaTime, oldPosition, newPosition, objElevation);
        playerIcon.UpdateArrowSpriteLocation();
    }


    private float sokobanBlockElevation => SokobanBoard.BLOCK_ELEVATION;


    private SokobanBlock GetBlock(SVector2Int playerNewPosition)
    {
        return sokobanBoard.boardInfo.GetBlock(playerNewPosition);
    }

    private bool PositionHasBlock(SVector2Int blockPosition)
    {
        return sokobanBoard.boardInfo.PositionHasBlock(blockPosition);
    }

    private SVector2Int GetCurrentPosition()
    {
        Debug.Assert(currentGridPosition != null, nameof(currentGridPosition) + " != null");
        return (SVector2Int) currentGridPosition;
    }

    public bool CanMove(PlayerDirection directionFromKeyCode)
    {
        // If the player is on the border, they cannot move.
        var newPosition = GetCurrentPosition() + SokobanBoardInfo.GetVectorInDirection(directionFromKeyCode);
        if (!SokobanBoardInfo.PositionIsOnBoard(newPosition))
        {
            return false;
        }

        // If the player is being blocked by a block, then prevent movement.
        return !PlayerIsBlockedByBlock(newPosition, directionFromKeyCode);
    }

    private bool PlayerIsBlockedByBlock(SVector2Int newPosition, PlayerDirection directionFromKeyCode)
    {
        // Check if there is actually a block at the targeted position.
        if (!sokobanBoard.boardInfo.PositionHasBlock(newPosition))
        {
            return false;
        }

        // Case 1: The block is being impeded by another block.
        var positionRelativeToNewPositionInDirection =
            sokobanBoard.boardInfo.GetPositionRelativeToInDirection(newPosition, directionFromKeyCode);
        if (sokobanBoard.boardInfo.PositionHasBlock(positionRelativeToNewPositionInDirection))
        {
            return true;
        }

        // Case 2: The block is being impeded by the edge of the board.
        return !SokobanBoardInfo.PositionIsOnBoard(positionRelativeToNewPositionInDirection);
    }
}