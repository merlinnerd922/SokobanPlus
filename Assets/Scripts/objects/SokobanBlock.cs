using System.Collections.Generic;
using UnityEngine;

public class SokobanBlock : SokobanMovable
{
    public override void SetSokobanPosition(Vector2Int blockNewPosition)
    {
        sokobanBoard.boardInfo.RemoveEmptySlot(blockNewPosition);

        if (currentGridPosition != null)
        {
            sokobanBoard.boardInfo.blockPositions.Remove((Vector2Int) currentGridPosition);
        }
        sokobanBoard.boardInfo.blockPositions[blockNewPosition] = this;
        base.SetSokobanPosition(blockNewPosition);
    }

    public void InitBlock(int i, int j, SokobanGameManager sokobanGameManager)
    {
        Init(sokobanGameManager, new Vector2Int(i, j));
    }
}
