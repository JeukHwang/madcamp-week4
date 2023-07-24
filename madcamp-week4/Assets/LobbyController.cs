using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyController : MonoBehaviour
{
    public GameObject mainCanvas;
    public GameObject playCanvas;
    public GameObject workshopCanvas;
    public GameObject settingsCanvas;

    public GameObject[] mainCanvasButtons;
    public GameObject[] playCanvasButtons;
    public GameObject[] workshopCanvasButtons;
    public GameObject[] settingsCanvasButtons;

    public void Start()
    {
        foreach (GameObject button in mainCanvasButtons)
        {
            button.GetComponent<Button>().onClick.AddListener(showMainCanvas);
        }
        foreach (GameObject button in playCanvasButtons)
        {
            button.GetComponent<Button>().onClick.AddListener(showPlayCanvas);
        }
        foreach (GameObject button in workshopCanvasButtons)
        {
            button.GetComponent<Button>().onClick.AddListener(showWorkshopCanvas);
        }
        foreach (GameObject button in settingsCanvasButtons)
        {
            button.GetComponent<Button>().onClick.AddListener(() => { showSettingsCanvas(); });
        }
        showMainCanvas();
    }

    public void showMainCanvas()
    {
        mainCanvas.SetActive(true);
        playCanvas.SetActive(false);
        workshopCanvas.SetActive(false);
        settingsCanvas.SetActive(false);
    }

    public void showPlayCanvas()
    {
        playCanvas.SetActive(true);
        mainCanvas.SetActive(false);
        workshopCanvas.SetActive(false);
        settingsCanvas.SetActive(false);
    }

    public void showWorkshopCanvas()
    {
        workshopCanvas.SetActive(true);
        mainCanvas.SetActive(false);
        playCanvas.SetActive(false);
        settingsCanvas.SetActive(false);
    }

    public void showSettingsCanvas()
    {
        settingsCanvas.SetActive(true);
        mainCanvas.SetActive(false);
        playCanvas.SetActive(false);
        workshopCanvas.SetActive(false);
    }
}
