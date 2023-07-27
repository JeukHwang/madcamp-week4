using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class Tutorial : MonoBehaviour
{
    // 맨 처음 페이드 인 효과
    public GameObject FadeInCanvas;
    // 클리어 시 페이드 아웃 효과
    public GameObject FadeOutCanvas;

    // 안내사항 텍스트
    public GameObject InfoPanel;
    public GameObject InfoText;
    TMP_Text infoText;

    // 문
    public GameObject Doors;
    // 스위치
    public GameObject Switches;
    // 플레이어
    public GameObject player;

    float stageTime;
    float totalTime;
    float exitTime;
    int stage;

    bool isExit;

    // Start is called before the first frame update
    void Start()
    {
        stageTime = 0;
        totalTime = 0;
        exitTime = 0;
        stage = 0;
        isExit = false;

        infoText = InfoText.GetComponent<TMP_Text>();

        FadeInCanvas.SetActive(true);
        FadeOutCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        stageTime += Time.deltaTime;
        totalTime += Time.deltaTime;
        // print(stageTime);

        switch (stage)
        {
            case 0: // 페이드 인
                if (stageTime < 2)
                {
                    break;
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
                    stage = 1;
                    stageTime = 0;
                }
                break;

            case 1: // 방향키, Shift, F, L
                if (stageTime < 2) break;
                else if (2 < stageTime && stageTime < 2.1f) ActivateInfo("방향키로 플레이어를 움직일 수 있습니다.");
                else if (7 < stageTime && stageTime < 7.1f) InfoPanel.SetActive(false);

                else if (9 < stageTime && stageTime < 9.1f) ActivateInfo("Shift를 누르면 달릴 수 있습니다.");
                else if (13 < stageTime && stageTime < 13.1f) InfoPanel.SetActive(false);

                else if (15 < stageTime && stageTime < 15.1f) ActivateInfo("F를 누르면 토치를 주울 수 있습니다.");
                else if (20 < stageTime && stageTime < 20.1f) InfoPanel.SetActive(false);

                else if (22 < stageTime && stageTime < 22.1f) ActivateInfo("L을 누르면 토치를 끌 수 있습니다.");
                else if (24 < stageTime && stageTime < 24.1f) InfoPanel.SetActive(false);

                else if (27 < stageTime && stageTime < 27.1f)
                {
                    ActivateInfo("이제 다음 단계로 넘어가겠습니다.");
                    Switches.transform.GetChild(0).gameObject.GetComponent<TorchSwitchController>().red = false;
                }
                else if (32 < stageTime && stageTime < 32.1f) InfoPanel.SetActive(false);

                else if (32.2f < stageTime)
                {
                    stage = 2;
                    stageTime = 0;
                }
                break;

            case 2: // 토치와 스위치
                if (stageTime < 2) break;
                else if (2 < stageTime && stageTime < 2.1f) ActivateInfo("스위치가 활성화되면 문을 열 수 있습니다.");
                else if (7 < stageTime && stageTime < 7.1f) InfoPanel.SetActive(false);

                else if (9 < stageTime && stageTime < 9.1f) ActivateInfo("적절한 토치를 사용해 스위치를 활성화할 수 있습니다. \n(단, 토치가 스위치의 범위 내에 있어야 합니다.)");
                else if (14 < stageTime && stageTime < 14.1f) InfoPanel.SetActive(false);

                else if (16 < stageTime && stageTime < 16.1f) ActivateInfo("하얀색 토치를 하얀색 스위치에 가까이 두세요. \n(F를 누르면 토치를 버릴 수 있습니다.)");
                else if (21 < stageTime && stageTime < 21.1f) InfoPanel.SetActive(false);

                else if (23 < stageTime && stageTime < 23.1f)
                {
                    ActivateInfo("이제 다음 단계로 넘어가겠습니다.");
                    Switches.transform.GetChild(0).gameObject.GetComponent<TorchSwitchController>().red = false;
                }
                else if (27 < stageTime && stageTime < 27.1f) InfoPanel.SetActive(false);

                else if (27.2f < stageTime)
                {
                    stage = 3;
                    stageTime = 0;
                }
                break;

            case 3: // 둘 이상의 토치
                if (stageTime < 2) break;
                else if (2 < stageTime && stageTime < 2.1f) ActivateInfo("어떤 스위치는 2개 이상의 토치가 주변에 있어야 활성화됩니다.");
                else if (7 < stageTime && stageTime < 7.1f) InfoPanel.SetActive(false);

                else if (9 < stageTime && stageTime < 9.1f) ActivateInfo("스위치 아랫부분의 색깔을 통해 어떤 빛이 필요한지 알 수 있습니다.");
                else if (13 < stageTime && stageTime < 13.1f) InfoPanel.SetActive(false);

                else if (15 < stageTime && stageTime < 15.1f) ActivateInfo("스위치 윗부분의 색깔을 통해 현재까지 어떤 빛이 만족되었는지 알 수 있습니다.");
                else if (19 < stageTime && stageTime < 19.1f) InfoPanel.SetActive(false);

                else if (21 < stageTime && stageTime < 21.1f) ActivateInfo("빨간색과 파란색 토치를 보라색 스위치에 가까이 두세요.");
                else if (26 < stageTime && stageTime < 26.1f) InfoPanel.SetActive(false);

                else if (28 < stageTime && stageTime < 28.1f)
                {
                    ActivateInfo("이제 다음 단계로 넘어가겠습니다.");
                    Switches.transform.GetChild(0).gameObject.GetComponent<TorchSwitchController>().red = false;
                }
                else if (32 < stageTime && stageTime < 32.1f) InfoPanel.SetActive(false);

                else if (32.2f < stageTime)
                {
                    stage = 4;
                    stageTime = 0;
                }
                break;

            case 4: // 투명 벽
                if (stageTime < 2) break;
                else if (2 < stageTime && stageTime < 2.1f) ActivateInfo("투명한 벽은 빛이 통과할 수 있습니다.");
                else if (6 < stageTime && stageTime < 6.1f) InfoPanel.SetActive(false);

                else if (8 < stageTime && stageTime < 8.1f) ActivateInfo("때문에 벽이 있더라도 토치를 가까이 하면 스위치가 활성화됩니다.");
                else if (13 < stageTime && stageTime < 13.1f) InfoPanel.SetActive(false);

                else if (15 < stageTime && stageTime < 15.1f) ActivateInfo("초록색 토치를 초록색 스위치에 가까이 두세요.");
                else if (19 < stageTime && stageTime < 19.1f) InfoPanel.SetActive(false);

                else if (21 < stageTime && stageTime < 21.1f)
                {
                    ActivateInfo("이제 다음 단계로 넘어가겠습니다.");
                    Switches.transform.GetChild(0).gameObject.GetComponent<TorchSwitchController>().red = false;
                }
                else if (24 < stageTime && stageTime < 24.1f) InfoPanel.SetActive(false);

                else if (24.2f < stageTime)
                {
                    stage = 5;
                    stageTime = 0;
                }
                break;

            case 5: // 게임 클리어
                if (stageTime < 2) break;
                else if (2 < stageTime && stageTime < 2.1f) ActivateInfo("초록색 스포트라이트는 출구를 가리킵니다.");
                else if (6 < stageTime && stageTime < 6.1f) InfoPanel.SetActive(false);

                else if (8 < stageTime && stageTime < 8.1f) ActivateInfo("출구에 도착해서 튜토리얼을 클리어하세요.");
                else if (13 < stageTime && stageTime < 13.1f) InfoPanel.SetActive(false);
                break;

            case 6: // 페이드 아웃 효과
                if (stageTime < 3)
                {
                    FadeInCanvas.SetActive(false);
                    FadeOutCanvas.SetActive(true);
                    float opaque = stageTime / 6f;
                    FadeOutCanvas.transform.GetChild(0).GetComponent<Image>().color = new Color(0, 0, 0, opaque);
                }
                else if (5 < stageTime && stageTime < 6)
                {
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

                break;


        }

        if (isExit)
        {
            exitTime += Time.timeScale;

            if (exitTime > 2f)
            {
                // 플레이어의 입력을 막는다.
                player.GetComponent<PlayerInput>().enabled = false;
                isExit = false;

                // 페이드 아웃을 한다.
                stage = 6;
                stageTime = 0;
            }
        }
    }

    private void ActivateInfo(string msg)
    {
        InfoPanel.SetActive(true);
        infoText.text = msg;
    }

    public void OnTriggerEnter(Collider other)
    {
        //  충돌체가 플레이어가 아니라면, 함수를 실행하지 않는다.
        if (!other.gameObject.CompareTag("Player")) return;

        isExit = true;
    }

    public void OnTriggerExit(Collider other)
    {
        isExit = false;
        exitTime = 0;
    }
}
