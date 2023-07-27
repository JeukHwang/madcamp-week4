using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginFadeIn : MonoBehaviour
{
    public GameObject titleLight;
    public GameObject titleName;
    public GameObject others;
    public GameObject torchSwitch;

    float currentTime;

    private void Start()
    {
        titleLight.SetActive(false);
        titleName.SetActive(false);
        others.SetActive(false);
        torchSwitch.SetActive(false);
        currentTime = 0;
    }

    private void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime < 2)
        {
            titleLight.SetActive(true);
            float opaque = currentTime / 2f;
            titleLight.GetComponent<Image>().color = new Color(1, 1, 1, opaque);

        }
        else if (3 < currentTime && currentTime < 4)
        {
            torchSwitch.SetActive(true);
        }
        else if (5 < currentTime && currentTime < 6)
        {
            titleName.SetActive(true);
            float opaque = currentTime - 5f;
            titleName.GetComponent<Image>().color = new Color(1, 1, 1, opaque);
        }
        else if (7 < currentTime)
        {
            others.SetActive(true);
        }
    }
}
