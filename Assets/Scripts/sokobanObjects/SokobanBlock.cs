using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

public class SokobanBlock : SokobanMovable
{
    public override void SetSokobanPosition(SVector2Int positionToSet, SVector2Int previousPosition,
        MovementSetType setType)
    {
        if (setType == MovementSetType.MOVE)
        {
            boardInfo._boardData.blockPositions.Add(positionToSet);
        }
        
        boardInfo.blockPositions[positionToSet] = this;    
        sokobanBoard.boardInfo.RemoveEmptySlot(positionToSet);

        // In case the block moved here from another position.
        if (currentGridPosition != null)
        {
            boardInfo.RemoveBlock(currentGridPosition);
        }

        base.SetSokobanPosition(positionToSet, previousPosition, setType);

        if (IsOnReceptacle())
        {
            boardInfo.IncrementPressedSlots();
        }

        if (PositionIsReceptacle(previousPosition))
        {
            boardInfo.DecrementPressedSlots();
        }
        
        // If this was the last block that landed on a receptacle, declare victory for the player.
        if (AllReceptaclesPressed())
        {
            DeclareVictory();
        }
    }

    private SokobanBoardInfo boardInfo => sokobanBoard.boardInfo;

    private void DeclareVictory()
    {
        sokobanBoard.GetGameManager().DeclareVictory();
    }

    private bool AllReceptaclesPressed()
    {
        return sokobanBoard.boardInfo.GetPressedSlots() == SokobanBoardInfo.NUM_BOXES;
    }

    private bool PositionIsReceptacle(SVector2Int pos)
    {
        return sokobanBoard.boardInfo.ReceptaclesContain(pos);
    }

    private bool IsOnReceptacle()
    {
        return sokobanBoard.boardInfo.ReceptaclesContain(currentGridPosition);
    }

    public void InitBlock(int i, int j, SokobanGameManager sokobanGameManager)
    {
        Init(sokobanGameManager, new SVector2Int(i, j));
    }
}

public enum MovementSetType
{
    INSTANTIATE, MOVE
}
