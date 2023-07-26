using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButton : MonoBehaviour
{
    public void OnClickExitButton()
    {
        gameObject.SetActive(true);
    }

    public void OnClickYesButton()
    {
        Debug.Log("Yes");
    }

    public void OnClickNoButton()
    {
        gameObject.SetActive(false);
    }
}
