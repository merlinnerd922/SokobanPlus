using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SokobanGameManager : MonoBehaviour
{
    public SokobanBoard sokobanBoard;
    
    
    public void Start()
    {
        sokobanBoard.Initialize();
    }
}
