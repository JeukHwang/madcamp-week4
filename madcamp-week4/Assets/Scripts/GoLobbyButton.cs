using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoLobbyButton : MonoBehaviour
{
    public void OnClickGoLobby()
    {
        SceneManager.LoadScene("Lobby");
    }
}
