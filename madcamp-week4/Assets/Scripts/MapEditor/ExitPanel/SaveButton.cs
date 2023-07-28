using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class SaveButton : MonoBehaviour
{
    // 여기서의 gameObject는 CheckSaveToast을 가리킨다.
    public void OnClickSaveButton()
    {
        gameObject.SetActive(true);
    }

    public void OnClickYesButton(GameObject ConfirmSaveToast)
    {
        GameMap gameMap = ClassJSON.MapToJson(MapEditor.gameObjects, MapEditor.mapWidth, MapEditor.mapHeight, gameObject.transform.GetChild(0).GetComponent<TMP_InputField>().text);
        string json = JsonUtility.ToJson(gameMap);

        StartCoroutine(saveMap(json, ConfirmSaveToast));
    }

    public void OnClickNoButton()
    {
        gameObject.SetActive(false);
        // gameObject.transform.GetChild(0).GetComponent<TMP_InputField>().text = string.Empty; 제목 지우지 말고 그냥 냅둘까?
    }

    public void OnClickCloseButton(GameObject ConfirmSaveToast)
    {
        ConfirmSaveToast.SetActive(false);
        gameObject.SetActive(false);
    }

    private IEnumerator saveMap(string json, GameObject ConfirmSaveToast)
    {
        string baseUrl = "https://madcamp-week4-server-production.up.railway.app/";
        string username = PlayerPrefs.GetString("username");
        string password = PlayerPrefs.GetString("password");
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);
        form.AddField("name", gameObject.transform.GetChild(0).GetComponent<TMP_InputField>().text);
        form.AddField("json", json);
        UnityWebRequest www = UnityWebRequest.Post(baseUrl + "game/create", form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.result);
            Debug.Log(www.error);
            ConfirmSaveToast.SetActive(true);
            ConfirmSaveToast.transform.GetChild(0).GetComponent<TMP_Text>().text = "Save failed!";
        }
        else
        {
            Debug.Log("Form upload complete!");
            Debug.Log(www.result);
            Debug.Log(www.downloadHandler.text);
            ConfirmSaveToast.SetActive(true);
            ConfirmSaveToast.transform.GetChild(0).GetComponent<TMP_Text>().text = "Saved successfully!";
        }
    }
}