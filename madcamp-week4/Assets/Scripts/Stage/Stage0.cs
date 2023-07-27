using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class Tutorial : MonoBehaviour
{
    // �� ó�� ���̵� �� ȿ��
    public GameObject FadeInCanvas;
    // Ŭ���� �� ���̵� �ƿ� ȿ��
    public GameObject FadeOutCanvas;

    // �ȳ����� �ؽ�Ʈ
    public GameObject InfoPanel;
    public GameObject InfoText;
    TMP_Text infoText;

    // ��
    public GameObject Doors;
    // ����ġ
    public GameObject Switches;
    // �÷��̾�
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
            case 0: // ���̵� ��
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

            case 1: // ����Ű, Shift, F, L
                if (stageTime < 2) break;
                else if (2 < stageTime && stageTime < 2.1f) ActivateInfo("����Ű�� �÷��̾ ������ �� �ֽ��ϴ�.");
                else if (7 < stageTime && stageTime < 7.1f) InfoPanel.SetActive(false);

                else if (9 < stageTime && stageTime < 9.1f) ActivateInfo("Shift�� ������ �޸� �� �ֽ��ϴ�.");
                else if (13 < stageTime && stageTime < 13.1f) InfoPanel.SetActive(false);

                else if (15 < stageTime && stageTime < 15.1f) ActivateInfo("F�� ������ ��ġ�� �ֿ� �� �ֽ��ϴ�.");
                else if (20 < stageTime && stageTime < 20.1f) InfoPanel.SetActive(false);

                else if (22 < stageTime && stageTime < 22.1f) ActivateInfo("L�� ������ ��ġ�� �� �� �ֽ��ϴ�.");
                else if (24 < stageTime && stageTime < 24.1f) InfoPanel.SetActive(false);

                else if (27 < stageTime && stageTime < 27.1f)
                {
                    ActivateInfo("���� ���� �ܰ�� �Ѿ�ڽ��ϴ�.");
                    Switches.transform.GetChild(0).gameObject.GetComponent<TorchSwitchController>().red = false;
                }
                else if (32 < stageTime && stageTime < 32.1f) InfoPanel.SetActive(false);

                else if (32.2f < stageTime)
                {
                    stage = 2;
                    stageTime = 0;
                }
                break;

            case 2: // ��ġ�� ����ġ
                if (stageTime < 2) break;
                else if (2 < stageTime && stageTime < 2.1f) ActivateInfo("����ġ�� Ȱ��ȭ�Ǹ� ���� �� �� �ֽ��ϴ�.");
                else if (7 < stageTime && stageTime < 7.1f) InfoPanel.SetActive(false);

                else if (9 < stageTime && stageTime < 9.1f) ActivateInfo("������ ��ġ�� ����� ����ġ�� Ȱ��ȭ�� �� �ֽ��ϴ�. \n(��, ��ġ�� ����ġ�� ���� ���� �־�� �մϴ�.)");
                else if (14 < stageTime && stageTime < 14.1f) InfoPanel.SetActive(false);

                else if (16 < stageTime && stageTime < 16.1f) ActivateInfo("�Ͼ�� ��ġ�� �Ͼ�� ����ġ�� ������ �μ���. \n(F�� ������ ��ġ�� ���� �� �ֽ��ϴ�.)");
                else if (21 < stageTime && stageTime < 21.1f) InfoPanel.SetActive(false);

                else if (23 < stageTime && stageTime < 23.1f)
                {
                    ActivateInfo("���� ���� �ܰ�� �Ѿ�ڽ��ϴ�.");
                    Switches.transform.GetChild(0).gameObject.GetComponent<TorchSwitchController>().red = false;
                }
                else if (27 < stageTime && stageTime < 27.1f) InfoPanel.SetActive(false);

                else if (27.2f < stageTime)
                {
                    stage = 3;
                    stageTime = 0;
                }
                break;

            case 3: // �� �̻��� ��ġ
                if (stageTime < 2) break;
                else if (2 < stageTime && stageTime < 2.1f) ActivateInfo("� ����ġ�� 2�� �̻��� ��ġ�� �ֺ��� �־�� Ȱ��ȭ�˴ϴ�.");
                else if (7 < stageTime && stageTime < 7.1f) InfoPanel.SetActive(false);

                else if (9 < stageTime && stageTime < 9.1f) ActivateInfo("����ġ �Ʒ��κ��� ������ ���� � ���� �ʿ����� �� �� �ֽ��ϴ�.");
                else if (13 < stageTime && stageTime < 13.1f) InfoPanel.SetActive(false);

                else if (15 < stageTime && stageTime < 15.1f) ActivateInfo("����ġ ���κ��� ������ ���� ������� � ���� �����Ǿ����� �� �� �ֽ��ϴ�.");
                else if (19 < stageTime && stageTime < 19.1f) InfoPanel.SetActive(false);

                else if (21 < stageTime && stageTime < 21.1f) ActivateInfo("�������� �Ķ��� ��ġ�� ����� ����ġ�� ������ �μ���.");
                else if (26 < stageTime && stageTime < 26.1f) InfoPanel.SetActive(false);

                else if (28 < stageTime && stageTime < 28.1f)
                {
                    ActivateInfo("���� ���� �ܰ�� �Ѿ�ڽ��ϴ�.");
                    Switches.transform.GetChild(0).gameObject.GetComponent<TorchSwitchController>().red = false;
                }
                else if (32 < stageTime && stageTime < 32.1f) InfoPanel.SetActive(false);

                else if (32.2f < stageTime)
                {
                    stage = 4;
                    stageTime = 0;
                }
                break;

            case 4: // ���� ��
                if (stageTime < 2) break;
                else if (2 < stageTime && stageTime < 2.1f) ActivateInfo("������ ���� ���� ����� �� �ֽ��ϴ�.");
                else if (6 < stageTime && stageTime < 6.1f) InfoPanel.SetActive(false);

                else if (8 < stageTime && stageTime < 8.1f) ActivateInfo("������ ���� �ִ��� ��ġ�� ������ �ϸ� ����ġ�� Ȱ��ȭ�˴ϴ�.");
                else if (13 < stageTime && stageTime < 13.1f) InfoPanel.SetActive(false);

                else if (15 < stageTime && stageTime < 15.1f) ActivateInfo("�ʷϻ� ��ġ�� �ʷϻ� ����ġ�� ������ �μ���.");
                else if (19 < stageTime && stageTime < 19.1f) InfoPanel.SetActive(false);

                else if (21 < stageTime && stageTime < 21.1f)
                {
                    ActivateInfo("���� ���� �ܰ�� �Ѿ�ڽ��ϴ�.");
                    Switches.transform.GetChild(0).gameObject.GetComponent<TorchSwitchController>().red = false;
                }
                else if (24 < stageTime && stageTime < 24.1f) InfoPanel.SetActive(false);

                else if (24.2f < stageTime)
                {
                    stage = 5;
                    stageTime = 0;
                }
                break;

            case 5: // ���� Ŭ����
                if (stageTime < 2) break;
                else if (2 < stageTime && stageTime < 2.1f) ActivateInfo("�ʷϻ� ����Ʈ����Ʈ�� �ⱸ�� ����ŵ�ϴ�.");
                else if (6 < stageTime && stageTime < 6.1f) InfoPanel.SetActive(false);

                else if (8 < stageTime && stageTime < 8.1f) ActivateInfo("�ⱸ�� �����ؼ� Ʃ�丮���� Ŭ�����ϼ���.");
                else if (13 < stageTime && stageTime < 13.1f) InfoPanel.SetActive(false);
                break;

            case 6: // ���̵� �ƿ� ȿ��
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
                // �÷��̾��� �Է��� ���´�.
                player.GetComponent<PlayerInput>().enabled = false;
                isExit = false;

                // ���̵� �ƿ��� �Ѵ�.
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
        //  �浹ü�� �÷��̾ �ƴ϶��, �Լ��� �������� �ʴ´�.
        if (!other.gameObject.CompareTag("Player")) return;

        isExit = true;
    }

    public void OnTriggerExit(Collider other)
    {
        isExit = false;
        exitTime = 0;
    }
}
