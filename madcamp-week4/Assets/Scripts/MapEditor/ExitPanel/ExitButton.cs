using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitButton : MonoBehaviour
{
    public void OnClickExitButton()
    {
        gameObject.SetActive(true);
    }

    public void OnClickYesButton()
    {
        SceneManager.LoadScene("Lobby");
    }

    public void OnClickNoButton()
    {
        gameObject.SetActive(false);
    }
}
