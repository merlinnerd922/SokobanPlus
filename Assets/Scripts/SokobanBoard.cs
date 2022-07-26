using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Serialization;
using static DefaultNamespace.Extend;
using Object = UnityEngine.Object;
using Random = System.Random;

public class SokobanBoard : MonoBehaviour
{
    private const int BOARD_WIDTH = 10;
    private const int BOARD_LENGTH = 10;

    [FormerlySerializedAs("boardCubeOddPrefab")] [FormerlySerializedAs("boardCubePrefab")]
    public BoardCube floorCubeOddPrefab;

    [FormerlySerializedAs("boardCubeEvenPrefab")]
    public BoardCube floorCubeEvenPrefab;

    public Camera mainCamera;
    private int NUM_BOXES = 3;
    public BoxCube boxPrefab;
    private HashSet<Vector2Int> _emptySpots;
    public PlayerObject playerPrefab;
    private HashSet<Vector2Int> boxSlots;
    private BoxSlotPrefab boxSlotPrefab;


    public void Initialize()
    {
        InitializeEmptySpots();
        AllocateBoxSlots();
        GenerateFloorCubes();
        GenerateBoxes();
        GeneratePlayer();
    }

    private void InitializeEmptySpots()
    {
        _emptySpots = new HashSet<Vector2Int>();
        for (int i = 0; i < BOARD_WIDTH; i++)
        {
            for (int j = 0; j < BOARD_LENGTH; j++)
            {
                _emptySpots.Add(new Vector2Int(i, j));
            }
        }
    }

    private void AllocateBoxSlots()
    {
        boxSlots = GetRandomEmptySlots(NUM_BOXES);
    }

    private HashSet<Vector2Int> GetRandomEmptySlots(int numSlots)
    {
        return _emptySpots.GetRandomN(3);
    }

    private void GenerateBoxes()
    {
        for (int i = 0; i < NUM_BOXES; i++)
        {
            Vector2Int vec = GetRandomEmptySlot(useBoxSlots: true);
            GenerateBox(vec.x, vec.y);
        }
    }

    private void GenerateFloorCubes()
    {
        for (int i = 0; i < BOARD_WIDTH; i++)
        {
            for (int j = 0; j < BOARD_LENGTH; j++)
            {
                GenerateFloorCube(i, j);
            }
        }
    }

    private Vector2Int GetRandomEmptySlot(bool useBoxSlots)
    {
        return !useBoxSlots ? _emptySpots.Except(boxSlots).GetRandom() : _emptySpots.GetRandom();
    }

    private void GeneratePlayer()
    {
        Vector2Int vec = GetRandomEmptySlot(useBoxSlots: true);
        Instantiate(playerPrefab, new Vector3(vec.x, 1, vec.y), Quaternion.identity);
        _emptySpots.Remove(new Vector2Int(vec.x, vec.y));
    }

    private void GenerateBox(int i, int j)
    {
        Instantiate(boxPrefab, new Vector3(i, 1, j), Quaternion.identity);
        _emptySpots.Remove(new Vector2Int(i, j));
    }


    private void GenerateFloorCube(int i, int j)
    {
        if (boxSlots.Contains(new Vector2Int(i, j)))
        {
            Instantiate(boxSlotPrefab.gameObject, new Vector3(i, 0, j), Quaternion.identity, this.transform);
            return;
        }

        var prefabToGenerate = (i + j).IsOdd() ? floorCubeOddPrefab.gameObject : floorCubeEvenPrefab.gameObject;
        Instantiate(prefabToGenerate, new Vector3(i, 0, j), Quaternion.identity, this.transform);
    }
}