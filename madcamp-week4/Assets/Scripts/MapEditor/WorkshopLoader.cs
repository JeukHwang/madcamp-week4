using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WorkshopLoader : MonoBehaviour
{
    public GameObject PlaneFactory;
    public GameObject PlayerFactory;
    public GameObject LightFactory;
    public GameObject SwitchFactory;
    public GameObject DoorFactory;
    public GameObject ExitFactory;
    public GameObject WallFactory;

    public static string MapTitle = "";

    private string baseUrl = "https://madcamp-week4-server-production.up.railway.app/";

    private void Start()
    {
        string workshopId = PlayerPrefs.GetString("WorkshopId");
        Debug.Log("Load WorkshopId " + workshopId);
        StartCoroutine(loadMap(workshopId));
    }

    private IEnumerator loadMap(string id)
    {
        string username = PlayerPrefs.GetString("username");
        string password = PlayerPrefs.GetString("password");
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
        (GameObject[], string) map = ClassJSON.JsonToMap(str, PlaneFactory, PlayerFactory, LightFactory, SwitchFactory, DoorFactory, ExitFactory, WallFactory);
        GameObject[] gameObjects = map.Item1;
        MapTitle = map.Item2;
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