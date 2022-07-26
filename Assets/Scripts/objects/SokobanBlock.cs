using UnityEngine;

public class SokobanBlock : SokobanMovable
{
    public override void SetSokobanPosition(Vector2Int blockNewPosition)
    {
        sokobanBoard.RemoveEmptySlot(blockNewPosition);

        if (_currentPosition != null)
        {
            sokobanBoard._blockPositions.Remove((Vector2Int) _currentPosition);
        }
        sokobanBoard._blockPositions[blockNewPosition] = this;
        base.SetSokobanPosition(blockNewPosition);
    }

    public void InitBlock(int i, int j, SokobanGameManager sokobanGameManager)
    {
        this.Init(sokobanGameManager, new Vector2Int(i, j));
    }
}
