using System;
using System.Collections;
using System.Collections.Generic;
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

    [FormerlySerializedAs("boardCubeOddPrefab")] 
    [FormerlySerializedAs("boardCubePrefab")] 
    public BoardCube floorCubeOddPrefab;
    
    [FormerlySerializedAs("boardCubeEvenPrefab")] 
    public BoardCube floorCubeEvenPrefab;
    public Camera mainCamera;
    private int NUM_BOXES = 3;
    public BoxCube boxPrefab;

    public void Initialize()
    {
        for (int i = 0; i < BOARD_WIDTH; i++)
        {
            for (int j = 0; j < BOARD_LENGTH; j++)
            {
                GenerateFloorCube(i, j);
            }
        }

        for (int i = 0; i < NUM_BOXES; i++)
        {
            GenerateBox(GetRandInt(0, BOARD_WIDTH), GetRandInt(0, BOARD_LENGTH));
        }
    }

    private void GenerateBox(int i, int j)
    {
        Instantiate(boxPrefab, new Vector3(i, 1, j), Quaternion.identity);
    }


    private void GenerateFloorCube(int i, int j)
    {
        var prefabToGenerate = (i + j).IsOdd() ? floorCubeOddPrefab.gameObject : floorCubeEvenPrefab.gameObject;
        Instantiate(prefabToGenerate, new Vector3(i, 0, j), Quaternion.identity, this.transform);
    }


}
