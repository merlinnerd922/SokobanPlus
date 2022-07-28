using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

[Serializable]
public class SokobanBoardInfo
{
    internal SokobanBoardData _boardData;

    public Lazy<HashSet<Vector2Int>> corners = new(() => new HashSet<Vector2Int>
    {
        new(0, 0),
        new(0, BOARD_LENGTH - 1),
        new(BOARD_WIDTH - 1, 0),
        new(BOARD_WIDTH - 1, BOARD_LENGTH - 1)
    });

    private Dictionary<Vector2Int, SokobanBlock> blockPositions
    {
        get;
        set;
    }

    public PlayerObject thisPlayer;


    internal const int BOARD_WIDTH = 10;
    internal const int BOARD_LENGTH = 10;
    public const int NUM_BOXES = 3;

    public void InitializeEmptySpots()
    {
        _boardData.emptySpots = new HashSet<SVector2Int>();
        for (int i = 0; i < BOARD_WIDTH; i++)
        {
            for (int j = 0; j < BOARD_LENGTH; j++)
            {
                AddEmptySlot(i, j);
            }
        }
    }

    public void AddEmptySlot(int i, int j)
    {
        _boardData.emptySpots.Add(new Vector2Int(i, j));
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
        _boardData.emptySpots.Remove(playerPosition);
    }

    public Vector2Int GetRandomEmptySlot(SokobanBoard sokobanBoard,
        bool useBoxReceptacles = true, bool ignoreCorners = false)
    {
        HashSet<SVector2Int> theseSlots = new(_boardData.emptySpots);
        if (!useBoxReceptacles)
        {
            theseSlots.RemoveWhere(x => sokobanBoard.boardInfo._boardData.boxReceptaclePositions.Contains(x));
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

    public void AllocateBoxReceptacles()
    {
        _boardData.boxReceptaclePositions = GetRandomEmptySlots(numSlots: NUM_BOXES);
    }

    public HashSet<SVector2Int> GetRandomEmptySlots(int numSlots)
    {
        return _boardData.emptySpots.GetRandomN(numSlots);
    }

    public static SokobanBoardInfo GenerateBoard()
    {
        var boardInfo = new SokobanBoardInfo
        {
            _boardData = new SokobanBoardData()
        };
        boardInfo.InitializeEmptySpots();
        boardInfo.AllocateBoxReceptacles();
        return boardInfo;
    }

    public void AddBlock(Vector2Int blockNewPosition, SokobanBlock sokobanBlock)
    {
        this.blockPositions[blockNewPosition] = sokobanBlock;
        this._boardData.blockPositions.Add(blockNewPosition);
    }

    public void InitBlockPositions()
    {
        this.blockPositions = new Dictionary<Vector2Int, SokobanBlock>();
        this._boardData.blockPositions = new HashSet<SVector2Int>();
    }

    public void RemoveBlock(Vector2Int? gridPosition)
    {
        Debug.Assert(gridPosition != null, nameof(gridPosition) + " != null");
        this.blockPositions.Remove((Vector2Int) gridPosition);
        this._boardData.blockPositions.Remove(gridPosition);
    }
}