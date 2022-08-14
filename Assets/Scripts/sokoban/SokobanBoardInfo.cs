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

    public Lazy<HashSet<SVector2Int>> corners = new(() => new HashSet<SVector2Int>
    {
        new(0, 0),
        new(0, BOARD_LENGTH - 1),
        new(BOARD_WIDTH - 1, 0),
        new(BOARD_WIDTH - 1, BOARD_LENGTH - 1)
    });

    internal Dictionary<SVector2Int, SokobanBlock> blockPositions
    {
        get;
        set;
    }

    public PlayerObject thisPlayer;
    private int pressedSlots;


    internal const int BOARD_WIDTH = 10;
    internal const int BOARD_LENGTH = 10;
    public const int NUM_BOXES = 3;

    public void InitializeEmptySpots()
    {
        _boardData.emptySpots = new SHashSet<SVector2Int>();
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
        _boardData.emptySpots.Add(new SVector2Int(i, j));
    }

    public void AddEmptySlot(SVector2Int oldPosition)
    {
        AddEmptySlot(oldPosition.x, oldPosition.y);
    }

    public bool PositionHasBlock(SVector2Int newPosition)
    {
        return blockPositions.ContainsKey(newPosition);
    }

    public SVector2Int GetPositionRelativeToInDirection(SVector2Int newPosition, PlayerDirection directionFromKeyCode)
    {
        return newPosition + SokobanGameManager.DIRECTION_VECTOR_MAPPING[directionFromKeyCode];
    }

    public static bool PositionIsOnBoard(SVector2Int somePosition)
    {
        return somePosition.x.InRange(lowerBoundInclusive: 0, upperBoundExclusive: BOARD_WIDTH) &&
               somePosition.y.InRange(lowerBoundInclusive: 0, upperBoundExclusive: BOARD_LENGTH);
    }

    public static SVector2Int GetVectorInDirection(PlayerDirection dir)
    {
        return SokobanGameManager.DIRECTION_VECTOR_MAPPING[dir];
    }

    public SokobanBlock GetBlock(SVector2Int blockPosition)
    {
        return blockPositions[blockPosition];
    }

    public void RemoveEmptySlot(SVector2Int playerPosition)
    {
        if (!_boardData.emptySpots.Contains(playerPosition))
        {
            throw new InvalidOperationException("Cannot remove an empty slot that isn't present!");
        }

        _boardData.emptySpots.Remove(playerPosition);
    }

    public SVector2Int GetRandomEmptySlot(bool useBoxReceptacles = true, bool ignoreCorners = false)
    {
        HashSet<SVector2Int> theseSlots = new(_boardData.emptySpots);
        if (!useBoxReceptacles)
        {
            theseSlots.RemoveWhere(x => _boardData.boxReceptaclePositions.Contains(x));
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

    public SHashSet<SVector2Int> GetRandomEmptySlots(int numSlots)
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
        boardInfo.SetPressedSlots(0);
        boardInfo.SetPlayerInitialPosition();
        boardInfo.GenerateBoxPositions();
        return boardInfo;
    }

    internal void SetPressedSlots(int numPressedSlots)
    {
        pressedSlots = numPressedSlots;
    }

    public void InitBlockPositions()
    {
        this.blockPositions = new Dictionary<SVector2Int, SokobanBlock>();
        this._boardData.blockPositions = new SHashSet<SVector2Int>();
    }

    public void RemoveBlock(SVector2Int gridPosition)
    {
        Debug.Assert(gridPosition != null, nameof(gridPosition) + " != null");
        this.blockPositions.Remove((SVector2Int) gridPosition);
        this._boardData.blockPositions.Remove(gridPosition);
    }

    public int GetPressedSlots()
    {
        return pressedSlots;
    }

    public SVector2Int SetPlayerInitialPosition()
    {
        SVector2Int vec = this.GetRandomEmptySlot(useBoxReceptacles: true, ignoreCorners: false);
        this.RemoveEmptySlot(vec);
        _boardData.playerPosition = vec;
        return vec;
    }

    public void GenerateBoxPositions()
    {
        this._boardData.blockPositions = new SHashSet<SVector2Int>();
        for (int i = 0; i < SokobanBoardInfo.NUM_BOXES; i++)
        {
            SVector2Int vec = this.GetRandomEmptySlot(useBoxReceptacles: false, ignoreCorners: true);
            this._boardData.blockPositions.Add(vec);
        }
    }

    public bool ReceptaclesContain(SVector2Int currentGridPosition)
    {
        return this._boardData.boxReceptaclePositions.Contains(currentGridPosition);
    }

    public void IncrementPressedSlots()
    {
        this.SetPressedSlots(this.GetPressedSlots() + 1);
    }

    public void DecrementPressedSlots()
    {
        this.SetPressedSlots(this.GetPressedSlots() - 1);
    }
}