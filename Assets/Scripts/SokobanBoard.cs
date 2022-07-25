using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SokobanBoard : MonoBehaviour
{
    public BoardCube boardCubePrefab;
    
    public void Initialize()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                GenerateCube(i, j);
            }
        }
    }

    private void GenerateCube(int i, int j)
    {
        Instantiate(boardCubePrefab.gameObject, new Vector3(i, 0, j), Quaternion.identity);
    }
}
