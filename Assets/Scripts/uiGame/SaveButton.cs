using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class SaveButton : SokobanUIButton
{
    private SokobanGameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        var rectTransform = GetRectTransformAndAnchorToCanvas();
        rectTransform.anchoredPosition = new Vector2(150, 0);
        gameManager = FindObjectOfType<SokobanGameManager>();
    }
    

    public void SaveGame()
    {
        string file = Newtonsoft.Json.JsonConvert.SerializeObject(gameManager.sokobanBoard.boardInfo._boardData, Formatting.Indented);
        var persistentDataPath = Application.persistentDataPath + "/gameData.json";
        Debug.Log(persistentDataPath);
        File.WriteAllText(persistentDataPath, file);
    }

}
