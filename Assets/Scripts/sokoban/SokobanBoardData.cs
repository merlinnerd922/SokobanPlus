using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using System.Runtime.Serialization;

public class SokobanBoardData
{
    public HashSet<SVector2Int> emptySpots;

    public HashSet<SVector2Int> boxReceptaclePositions;

    public HashSet<SVector2Int> blockPositions;

    public SVector2Int playerPosition;
}