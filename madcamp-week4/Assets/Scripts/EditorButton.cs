using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EditorButton : MonoBehaviour
{
    public void OnClickEditorButton()
    {
        SceneManager.LoadScene("Editor");
    }
}
