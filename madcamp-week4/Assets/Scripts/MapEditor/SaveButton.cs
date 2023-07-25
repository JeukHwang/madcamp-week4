using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveButton : MonoBehaviour
{
    public void OnClickSaveButton()
    {
        GameMap gameMap = ClassJSON.MapToJson(MapEditor.gameObjects, MapEditor.mapWidth, MapEditor.mapHeight);

        Debug.Log(JsonUtility.ToJson(gameMap));
    }
}
