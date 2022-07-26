using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

[Serializable]
public class SokobanBoardInfo
{
    private readonly SokobanBoard _sokobanBoard;
    public HashSet<Vector2Int> emptySpots;
    public Lazy<HashSet<Vector2Int>> corners = new(() => new HashSet<Vector2Int>
    {
        new(0, 0),
        new(0, BOARD_LENGTH - 1),
        new(BOARD_WIDTH - 1, 0),
        new(BOARD_WIDTH - 1, BOARD_LENGTH - 1)
    });
    
    public Dictionary<Vector2Int, SokobanBlock> blockPositions;
    public HashSet<Vector2Int> boxReceptacles;
    
    public SokobanBoardInfo(SokobanBoard sokobanBoard)
    {
        _sokobanBoard = sokobanBoard;
    }

    internal const int BOARD_WIDTH = 10;
    internal const int BOARD_LENGTH = 10;
    public const int NUM_BOXES = 3;

    public void InitializeEmptySpots(SokobanBoard sokobanBoard)
    {
        emptySpots = new HashSet<Vector2Int>();
        for (int i = 0; i < BOARD_WIDTH; i++)
        {
            for (int j = 0; j < BOARD_LENGTH; j++)
            {
                sokobanBoard.boardInfo.AddEmptySlot(i, j);
            }
        }
    }

    public void AddEmptySlot(int i, int j)
    {
        emptySpots.Add(new Vector2Int(i, j));
    }

    public void AddEmptySlot(Vector2Int oldPosition)
    {
        AddEmptySlot(oldPosition.x, oldPosition.y);
    }

    public bool PositionHasBlock(Vector2Int newPosition)
    {
        return blockPositions.ContainsKey(newPosition);
    }

    public Vector2Int GetPositionRelativeToInDirection(Vector2Int newPosition, PlayerDirection directionFromKeyCode)
    {
        return newPosition + SokobanGameManager.DIRECTION_VECTOR_MAPPING[directionFromKeyCode];
    }

    public static bool PositionIsOnBoard(Vector2Int somePosition)
    {
        return somePosition.x.InRange(lowerBoundInclusive: 0, upperBoundExclusive: BOARD_WIDTH) &&
               somePosition.y.InRange(lowerBoundInclusive: 0, upperBoundExclusive: BOARD_LENGTH);
    }

    public static Vector2Int GetVectorInDirection(PlayerDirection dir)
    {
        return SokobanGameManager.DIRECTION_VECTOR_MAPPING[dir];
    }

    public SokobanBlock GetBlock(Vector2Int blockPosition)
    {
        return blockPositions[blockPosition];
    }

    public void RemoveEmptySlot(Vector2Int playerPosition)
    {
        emptySpots.Remove(playerPosition);
    }

    public Vector2Int GetRandomEmptySlot(SokobanBoard sokobanBoard,
        bool useBoxReceptacles = true, bool ignoreCorners = false)
    {
        HashSet<Vector2Int> theseSlots = new(emptySpots);
        if (!useBoxReceptacles)
        {
            theseSlots.RemoveWhere(x => sokobanBoard.boardInfo.boxReceptacles.Contains(x));
        }

        if (ignoreCorners)
        {
            theseSlots.RemoveWhere(x => corners.Value.Contains(x));
        }

        if (theseSlots.Count == 0)
        {
            throw new EmptyCollectionException("Unable to generate an empty slot!");
        }

        return theseSlots.GetRandom();
    }

    public void AllocateBoxReceptacles(SokobanBoard sokobanBoard)
    {
        boxReceptacles = sokobanBoard.boardInfo.GetRandomEmptySlots(numSlots: NUM_BOXES);
    }

    public HashSet<Vector2Int> GetRandomEmptySlots(int numSlots)
    {
        return emptySpots.GetRandomN(numSlots);
    }
}