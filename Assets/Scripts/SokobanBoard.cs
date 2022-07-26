using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Serialization;
using System.Numerics;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class SokobanBoard : MonoBehaviour
{
    [FormerlySerializedAs("boardCubeOddPrefab")] [FormerlySerializedAs("boardCubePrefab")]
    public BoardCube floorCubeOddPrefab;

    [FormerlySerializedAs("boardCubeEvenPrefab")]
    public BoardCube floorCubeEvenPrefab;

    public Camera mainCamera;
    public SokobanBlock boxPrefab;

    public PlayerObject playerPrefab;

    [FormerlySerializedAs("boxReceptacle")] 
    [FormerlySerializedAs("boxSlotPrefab")] 
    public BoxReceptacle boxReceptaclePrefab;

    private SokobanGameManager _sokobanGameManager;
    internal PlayerObject thisPlayer;

    internal const int BLOCK_ELEVATION = 1;

    public SokobanBoardInfo boardInfo;

    public void Init(SokobanGameManager sokobanGameManager)
    {
        _sokobanGameManager = sokobanGameManager;
        boardInfo = new SokobanBoardInfo(this);

        boardInfo.InitializeEmptySpots(this);

        boardInfo.AllocateBoxReceptacles(this);
        GenerateFloorCubes();
        GenerateBoxes();
        GeneratePlayer();
    }

    private void GenerateBoxes()
    {
        boardInfo.blockPositions = new Dictionary<Vector2Int, SokobanBlock>();
        for (int i = 0; i < SokobanBoardInfo.NUM_BOXES; i++)
        {
            Vector2Int vec = boardInfo.
                GetRandomEmptySlot(sokobanBoard: this, useBoxReceptacles: false, ignoreCorners: true);
            GenerateBox(vec.x, vec.y);
        }
    }

    private void GenerateFloorCubes()
    {
        for (int i = 0; i < SokobanBoardInfo.BOARD_WIDTH; i++)
        {
            for (int j = 0; j < SokobanBoardInfo.BOARD_LENGTH; j++)
            {
                GenerateFloorCube(i, j);
            }
        }
    }

    private void GeneratePlayer()
    {
        Vector2Int vec = boardInfo.GetRandomEmptySlot(sokobanBoard:this, useBoxReceptacles: true, ignoreCorners: this);
        thisPlayer = Instantiate(playerPrefab, new Vector3(vec.x, 1, vec.y), Quaternion.identity);
        Vector2Int playerPosition = new Vector2Int(vec.x, vec.y);
        boardInfo.RemoveEmptySlot(playerPosition);

        thisPlayer.Init(_sokobanGameManager, playerPosition);
    }

    private void GenerateBox(int i, int j)
    {
        SokobanBlock newPrefab = Instantiate(boxPrefab, new Vector3(i, BLOCK_ELEVATION, j), Quaternion.identity);
        newPrefab.InitBlock(i, j, _sokobanGameManager);
    }


    private void GenerateFloorCube(int i, int j)
    {
        if (boardInfo.boxReceptacles.Contains(new Vector2Int(i, j)))
        {
            Instantiate(boxReceptaclePrefab.gameObject, new Vector3(i, 0, j), Quaternion.identity, transform);
            return;
        }

        var prefabToGenerate = (i + j).IsOdd() ? floorCubeOddPrefab.gameObject : floorCubeEvenPrefab.gameObject;
        Instantiate(prefabToGenerate, new Vector3(i, 0, j), Quaternion.identity, transform);
    }
}