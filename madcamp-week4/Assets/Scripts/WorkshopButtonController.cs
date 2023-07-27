using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System;

public class WorkshopButtonController : MonoBehaviour
{
    public GameObject levelButton;

    private string baseUrl = "https://madcamp-week4-server-production.up.railway.app/";
    private Vector2 buttonSize = new Vector2(500, 200);

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<GridLayoutGroup>().cellSize = buttonSize;
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        // Will cause error if WorkshopCanvas setActive(true) in editor
        StartCoroutine(Browse());
    }

    private IEnumerator Browse()
    {
        string username = PlayerPrefs.GetString("username");
        string password = PlayerPrefs.GetString("password");

        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);
        UnityWebRequest www = UnityWebRequest.Post(baseUrl + "game/browse", form);
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
            string data = www.downloadHandler.text;
            GameJSON[] gameJSONs = JsonUtility.FromJson<WrapperForGameJSONArray>("{ \"array\": " + data + "}").array;
            makeButton(gameJSONs);
        }
    }

    private void makeButton(GameJSON[] gameJSONs)
    {
        Debug.Log("MakeButton" + gameJSONs.Length);
        foreach (GameJSON gameJSON in gameJSONs)
        {
            Debug.Log("GameJSON" + gameJSON.name);

            GameObject button = Instantiate(levelButton);
            button.transform.parent = transform;
            RectTransform rectTransform = button.GetComponent<RectTransform>();
            rectTransform.localScale = new Vector3(1, 1, 1);
            Text text = button.GetComponentInChildren<Text>();
            DateTime dateTime = DateTime.Parse(gameJSON.createdAt, null, System.Globalization.DateTimeStyles.RoundtripKind);

            text.text = $"{gameJSON.name}\n{gameJSON.creatorId} | {dateTime.ToString("yyyy-MM-dd")}";
            text.GetComponent<RectTransform>().sizeDelta = buttonSize;
            string id = gameJSON.id;
            button.GetComponent<Button>().onClick.AddListener(() =>
            {
                Debug.Log("Save WorkshopId " + id);
                PlayerPrefs.SetString("WorkshopId", id);
                SceneManager.LoadScene("Workshop");
            });
        }
    }
}


[Serializable]
public class WrapperForGameJSONArray
{
    public GameJSON[] array;
}