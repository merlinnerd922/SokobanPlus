using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Serialization;

public class SokobanBoard : MonoBehaviour
{
    private const int BOARD_WIDTH = 10;
    private const int BOARD_LENGTH = 10;

    [FormerlySerializedAs("boardCubePrefab")] 
    public BoardCube boardCubeOddPrefab;
    
    public BoardCube boardCubeEvenPrefab;
    public Camera mainCamera;
    
    public void Initialize()
    {
        for (int i = 0; i < BOARD_WIDTH; i++)
        {
            for (int j = 0; j < BOARD_LENGTH; j++)
            {
                GenerateCube(i, j);
            }
        }

        var boardBounds = gameObject.GetMaxBounds();

        mainCamera.SetHeight(10);
        mainCamera.SetRotation(0, -90, 0);
    }

    private void GenerateCube(int i, int j)
    {
        var original = (i + j).IsOdd() ? boardCubeOddPrefab.gameObject : boardCubeEvenPrefab.gameObject;
        Instantiate(original, new Vector3(i, 0, j), Quaternion.identity, this.transform);
    }


}
