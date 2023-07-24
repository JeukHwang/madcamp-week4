using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class LoginController : MonoBehaviour
{
    public TMP_InputField usernameField;
    public TMP_InputField passwordField;

    private string baseUrl = "https://madcamp-week4-server-production.up.railway.app/";

    public void GrabFromInputField()
    {
        //List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        //formData.Add(new MultipartFormDataSection("field1=foo&field2=bar"));
        //formData.Add(new MultipartFormFileSection("my file data", "myfile.txt"));

        StartCoroutine(Login());
    }

    private IEnumerator Login()
    {
        string username = usernameField.text;
        string password = passwordField.text;
        if (username == "" || password == "")
        {
            ToastMsg.Instance.showMessage("Enter username and password", 1.0f);
            yield break;
        }
        Debug.Log("Send request");
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);
        UnityWebRequest www = UnityWebRequest.Post(baseUrl + "auth", form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            ToastMsg.Instance.showMessage("Username is already used or password is incorrect", 1.0f);
            Debug.Log(www.result);
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
            Debug.Log(www.result);
            Debug.Log(www.downloadHandler.text);
            PlayerPrefs.SetString("username", username);
            PlayerPrefs.SetString("password", password);
            SceneManager.LoadScene("Lobby");
        }
    }
}
