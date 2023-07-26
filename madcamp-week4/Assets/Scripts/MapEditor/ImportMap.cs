using Palmmedia.ReportGenerator.Core.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ImportMap : MonoBehaviour
{
    public GameObject PlaneFactory;
    public GameObject PlayerFactory;
    public GameObject LightFactory;
    public GameObject SwitchFactory;
    public GameObject DoorFactory;
    public GameObject ExitFactory;
    public GameObject WallFactory;


    private void Start()
    {
        StartCoroutine(loadMap("b8c4d746-faf9-4d45-8363-d7ea4b553416"));
    }

    private IEnumerator loadMap(string id)
    {
        string baseUrl = "https://madcamp-week4-server-production.up.railway.app/";
        string username = "jeuk";
        string password = "1234";
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);
        UnityWebRequest www = UnityWebRequest.Post(baseUrl + "game/id/" + id, form);
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

            string gameJson = www.downloadHandler.text;
            string mapJson = JsonUtility.FromJson<GameJSON>(gameJson).json;
            loadFromString(mapJson);
        }
    }

    private GameObject[] loadFromString(string str)
    {
        GameObject[] gameObjects = ClassJSON.JsonToMap(str, PlaneFactory, PlayerFactory, LightFactory, SwitchFactory, DoorFactory, ExitFactory, WallFactory);
        return gameObjects;
    }
}
[Serializable]
public class GameJSON
{
    public string id;
    public string name;
    public string createdAt;
    public string json;
    public string creatorId;
}