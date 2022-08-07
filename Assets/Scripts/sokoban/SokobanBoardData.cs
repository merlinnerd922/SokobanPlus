using System;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using System.Runtime.Serialization;

[Serializable]
public class SokobanBoardData
{
    public SHashSet<SVector2Int> emptySpots;

    public SHashSet<SVector2Int> boxReceptaclePositions;

    public SHashSet<SVector2Int> blockPositions;

    public SVector2Int playerPosition;
}