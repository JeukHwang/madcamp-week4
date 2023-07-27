using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class Stage1 : MonoBehaviour
{
    // 맨 처음 페이드 인 효과
    public GameObject FadeInCanvas;

    float stageTime;
    bool isFadeIn;


    // Start is called before the first frame update
    void Start()
    {
        FadeInCanvas.SetActive(true);
        stageTime = 0;
        isFadeIn = true;
    }

    // Update is called once per frame
    void Update()
    {
        stageTime += Time.deltaTime;

        if (isFadeIn)
        {
            if (stageTime < 2)
            {
                return;
            }
            else if (stageTime < 4)
            {
                FadeInCanvas.transform.GetChild(2).transform.position += Vector3.right * 200 * Time.deltaTime;
            }
            else if (stageTime < 6)
            {
                FadeInCanvas.transform.GetChild(3).transform.position += Vector3.right * 200 * Time.deltaTime;
            }
            else if (stageTime < 10)
            {
                float opaque = 1 - (stageTime - 6) / 4f;
                FadeInCanvas.transform.GetChild(0).GetComponent<Image>().color = new Color(0, 0, 0, opaque);
                FadeInCanvas.transform.GetChild(1).GetComponent<TMP_Text>().alpha = opaque;
                FadeInCanvas.transform.GetChild(2).gameObject.SetActive(false);
                FadeInCanvas.transform.GetChild(3).gameObject.SetActive(false);
            }
            else
            {
                FadeInCanvas.SetActive(false);
                isFadeIn = false;
                stageTime = 0;
            }
        }
    }
}
