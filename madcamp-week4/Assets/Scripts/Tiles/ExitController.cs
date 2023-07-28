using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ExitController : MonoBehaviour
{
    // 클리어 시 페이드 아웃 효과
    GameObject FadeOutCanvas;
    GameObject player;

    float totalTime;
    float stageTime;
    bool isExit;


    // Start is called before the first frame update
    void Start()
    {
        FadeOutCanvas = GameObject.Find("FadeOut");
        player = GameObject.FindWithTag("Player");

        totalTime = 0;
        stageTime = 0;
        isExit = false;
    }

    // Update is called once per frame
    void Update()
    {
        totalTime += Time.deltaTime;
        stageTime += Time.deltaTime;

        if (isExit)
        {
            if (stageTime < 3)
            {
                FadeOutCanvas.SetActive(true);
                float opaque = stageTime / 6f;
                FadeOutCanvas.transform.GetChild(0).GetComponent<Image>().color = new Color(0, 0, 0, opaque);
            }
            else if (5 < stageTime && stageTime < 6)
            {
                FadeOutCanvas.transform.GetChild(1).GetComponent<TMP_Text>().text = "<Workshop> \"" + WorkshopLoader.MapTitle + "\"";
                float opaque = stageTime - 5f;
                FadeOutCanvas.transform.GetChild(1).gameObject.SetActive(true);
                FadeOutCanvas.transform.GetChild(1).GetComponent<TMP_Text>().alpha = opaque;
            }
            else if (7 < stageTime && stageTime < 8)
            {
                float opaque = stageTime - 7f;
                FadeOutCanvas.transform.GetChild(2).gameObject.SetActive(true);
                FadeOutCanvas.transform.GetChild(2).GetComponent<TMP_Text>().alpha = opaque;
            }
            else if (10 < stageTime && stageTime < 11)
            {
                float opaque = stageTime - 10f;
                FadeOutCanvas.transform.GetChild(3).gameObject.SetActive(true);
                FadeOutCanvas.transform.GetChild(3).GetComponent<TMP_Text>().alpha = opaque;
            }
            else if (12 < stageTime && stageTime < 14)
            {
                float opaque = stageTime - 12f;
                FadeOutCanvas.transform.GetChild(4).gameObject.SetActive(true);
                FadeOutCanvas.transform.GetChild(4).GetComponent<TMP_Text>().alpha = opaque;
                FadeOutCanvas.transform.GetChild(4).GetComponent<TMP_Text>().text = (Mathf.Floor(totalTime * 100f) / 100f).ToString();
            }
            else if (15 < stageTime && stageTime < 16)
            {
                float opaque = stageTime - 15f;
                FadeOutCanvas.transform.GetChild(5).gameObject.SetActive(true);
                FadeOutCanvas.transform.GetChild(5).GetComponent<Image>().color = new Color(1, 1, 1, opaque * 3f / 4f);
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        //  충돌체가 플레이어가 아니라면, 함수를 실행하지 않는다.
        if (!other.gameObject.CompareTag("Player")) return;

        player.GetComponent<PlayerInput>().enabled = false;

        stageTime = 0;
        isExit = true;
    }
}
