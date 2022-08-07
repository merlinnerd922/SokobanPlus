using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

public class SokobanBlock : SokobanMovable
{
    public override void SetSokobanPosition(SVector2Int positionToSet, SVector2Int previousPosition)
    {
        sokobanBoard.boardInfo.RemoveEmptySlot(positionToSet);

        // In case the block moved here from another position.
        if (currentGridPosition != null)
        {
            sokobanBoard.boardInfo.RemoveBlock(currentGridPosition);
        }

        sokobanBoard.boardInfo.AddBlock(positionToSet, this);
        base.SetSokobanPosition(positionToSet, previousPosition);

        if (IsOnReceptacle())
        {
            sokobanBoard.IncrementPressedSlots();
        }

        if (PositionIsReceptacle(previousPosition))
        {
            sokobanBoard.DecrementPressedSlots();
        }
        
        // If this was the last block that landed on a receptacle, declare victory for the player.
        if (AllReceptaclesPressed())
        {
            DeclareVictory();
        }
    }

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
        return sokobanBoard.ReceptaclesContain(pos);
    }

    private bool IsOnReceptacle()
    {
        return sokobanBoard.ReceptaclesContain(currentGridPosition);
    }

    public void InitBlock(int i, int j, SokobanGameManager sokobanGameManager)
    {
        Init(sokobanGameManager, new SVector2Int(i, j));
    }
}
