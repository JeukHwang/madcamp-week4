using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class SaveButton : MonoBehaviour
{
    public void OnClickSaveButton()
    {
        GameMap gameMap = ClassJSON.MapToJson(MapEditor.gameObjects, MapEditor.mapWidth, MapEditor.mapHeight);
        string json = JsonUtility.ToJson(gameMap);

        StartCoroutine(saveMap(json));
    }

    private IEnumerator saveMap(string json)
    {
        string baseUrl = "https://madcamp-week4-server-production.up.railway.app/";
        string username = "jeuk";
        string password = "1234";
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);
        form.AddField("name", "Test Stage");
        form.AddField("json", json);
        UnityWebRequest www = UnityWebRequest.Post(baseUrl + "game/create", form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.result);
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
            Debug.Log(www.result);
            Debug.Log(www.downloadHandler.text);
        }
    }
}