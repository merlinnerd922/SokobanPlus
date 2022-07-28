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

    internal const int BLOCK_ELEVATION = 1;

    public SokobanBoardInfo boardInfo;

    public void Init(SokobanGameManager sokobanGameManager)
    {
        _sokobanGameManager = sokobanGameManager;
        boardInfo = SokobanBoardInfo.GenerateBoard();

        GenerateFloorTiles();
        GenerateBoxes();
        GeneratePlayer();
    }

    private void GenerateBoxes()
    {
        boardInfo.InitBlockPositions();

        for (int i = 0; i < SokobanBoardInfo.NUM_BOXES; i++)
        {
            Vector2Int vec = boardInfo.
                GetRandomEmptySlot(sokobanBoard: this, useBoxReceptacles: false, ignoreCorners: true);
            
            
            GenerateBox(vec.x, vec.y);
        }
    }

    private void GenerateFloorTiles()
    {
        for (int i = 0; i < SokobanBoardInfo.BOARD_WIDTH; i++)
        {
            for (int j = 0; j < SokobanBoardInfo.BOARD_LENGTH; j++)
            {
                GenerateFloorTile(i, j);
            }
        }
    }

    private void GeneratePlayer()
    {
        Vector2Int vec = boardInfo.GetRandomEmptySlot(sokobanBoard:this, useBoxReceptacles: true, ignoreCorners: this);
        boardInfo.thisPlayer = Instantiate(playerPrefab, new Vector3(vec.x, 1, vec.y), Quaternion.identity);
        
        Vector2Int playerPosition = new Vector2Int(vec.x, vec.y);
        boardInfo.RemoveEmptySlot(playerPosition);

        boardInfo.thisPlayer.Init(_sokobanGameManager, playerPosition);
        boardInfo.thisPlayer.SetParent(this);
    }

    private void GenerateBox(int i, int j)
    {
        SokobanBlock newBlock = Instantiate(boxPrefab, new Vector3(i, BLOCK_ELEVATION, j), Quaternion.identity);
        newBlock.InitBlock(i, j, _sokobanGameManager);
        newBlock.SetParent(this);
    }


    private void GenerateFloorTile(int i, int j)
    {
        if (boardInfo._boardData.boxReceptaclePositions.Contains(new Vector2Int(i, j)))
        {
            var boxReceptacle = Instantiate(boxReceptaclePrefab, new Vector3(i, 0, j), Quaternion.identity, transform);
            boxReceptacle.SetParent(this);
            return;
        }

        var prefabToGenerate = (i + j).IsOdd() ? floorCubeOddPrefab : floorCubeEvenPrefab;
        var newFloorTile = Instantiate(prefabToGenerate, new Vector3(i, 0, j), Quaternion.identity, transform);
        newFloorTile.SetParent(this);
    }
}