using System.Collections.Generic;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

public class SokobanBlock : SokobanMovable
{
    public override void SetSokobanPosition(Vector2Int blockNewPosition)
    {
        sokobanBoard.boardInfo.RemoveEmptySlot(blockNewPosition);

        if (currentGridPosition != null)
        {
            sokobanBoard.boardInfo.RemoveBlock(currentGridPosition);
        }

        sokobanBoard.boardInfo.AddBlock(blockNewPosition, this);
        base.SetSokobanPosition(blockNewPosition);
    }

    public void InitBlock(int i, int j, SokobanGameManager sokobanGameManager)
    {
        Init(sokobanGameManager, new Vector2Int(i, j));
    }
}
