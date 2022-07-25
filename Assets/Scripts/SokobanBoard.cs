using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SokobanBoard : MonoBehaviour
{
    public void Initialize()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                GenerateCube();
            }
        }
    }

    private void GenerateCube()
    {
        
    }
}
