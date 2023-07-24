using Palmmedia.ReportGenerator.Core.CodeAnalysis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapEditor : MonoBehaviour
{
    // ����ڰ� �ʿ��� ������ ������Ʈ (ȸ�� �� ���� ���) (�� �� Ŭ���ϸ� �ȴ�)
    public static GameObject userSelectedTarget = null;

    // ����ڰ� �ʿ��� ���콺�� ��� ������ �ִ� ������Ʈ (�̵� ���) (�� ������ �־�� �Ѵ�)
    GameObject userKeepedTarget = null;

    // ����ڰ� item panel���� ���콺�� ��� ������ �ִ� ������Ʈ (UI�� item ��ư�� ����Ų��)
    GameObject userSelectedItemButton = null;

    // ����ڰ� item panel���� ���콺�� �������� �ʿ� ������ ������ ������Ʈ (������ ������ ������Ʈ�� ����Ų��) (factory���� instantiate�Ѵ�)
    GameObject userCreateObject = null;

    public GameObject deleteButton;

    // ���콺 ��ġ Ʈ��ŷ��
    Ray ray;
    RaycastHit hit;

    // �巡�� �� ������� create�� �� �ʿ��ϴ�.
    public GameObject PlayerFactory;
    public GameObject LightFactory;
    public GameObject SwitchFactory;
    public GameObject DoorFactory;
    public GameObject ExitFactory;
    public GameObject WallFactory;
    public GameObject Factory6;
    public GameObject Factory7;

    // ���� �����ϴ� �г�
    public GameObject ColorPanel;

    public GameObject itemPanel;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // ���콺 ���� ��ư Ŭ���� ����
        if (Input.GetMouseButtonDown(0))
        {
            // if���� ������Ű��, UI ��� ���� �ƴϴ�. (�� ����)
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("UI ���� �ƴϴ�.");

                // ���� item panel���� Ŭ���ߴ� ���� �־�����, �����Ѵ�.
                if (ItemPanel.selectedItem != null) itemPanel.GetComponent<ItemPanel>().OnClickItemButton(null);

                // ���콺 Ŭ���� ȭ�� ��ǥ�� ����ĳ��Ʈ�� �����Ͽ� 3D ���� ��ǥ�� ���
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit)) // ����ĳ��Ʈ�� �ƹ� ��ü�� �ε��� ���
                {
                    Debug.Log("�ε��� 3D ���� ��ǥ: " + hit.point);

                    // ���� Ŭ���� ����� �ٴ��� ��쿡�� (���� ����� �ʱ�ȭ�Ѵ�.
                    if (hit.collider.gameObject.CompareTag("Plane"))
                    {
                        userSelectedTarget = null;
                        deleteButton.GetComponent<Button>().interactable = false;

                        ColorPanel.transform.GetChild(1).gameObject.SetActive(false);
                        ColorPanel.transform.GetChild(2).gameObject.SetActive(false);
                    }
                    else // �׷��� ���� ��� ����� �����Ѵ�. (����, �̵�, ���� ��� Ȱ��ȭ)
                    { 
                        userSelectedTarget = hit.collider.gameObject;
                        userKeepedTarget = hit.collider.gameObject;
                        deleteButton.GetComponent<Button>().interactable = true;
                    }
                }
            }
            // UI ��Ҹ� �����ߴ�. (�������� �巡�� �� ������� instantiate�� �� �ְ� �Ѵ�)
            else
            {
                // item panel ������ Ŭ���� ��ư�� ����Ų��.
                userSelectedItemButton = EventSystem.current.currentSelectedGameObject;
                Debug.Log(userSelectedItemButton.name);
            } 
        }

        if (Input.GetMouseButton(0)) 
        {
            // �� ������ ����� Ŭ���� ������ ���콺�� ��� ������ �ִ�.
            if (userKeepedTarget != null)
            {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    // �����̴� ���콺 ��ġ�� ����� �̵���Ų��.
                    userKeepedTarget.transform.position = normalizePosition(hit.point.x, 0, hit.point.z);
                }
            }
            // item panel ������ �������� Ŭ���� ������ ���콺�� ��� ������ �ִ�.
            else if (userSelectedItemButton != null)
            {
                // ���콺�� �� ���� ���·� ���콺 ��ġ�� �� ���� ���ʷ� �̵��ߴ�.
                if (userCreateObject == null && !EventSystem.current.IsPointerOverGameObject())
                {
                    GameObject instantiateTarget = null;

                    switch (userSelectedItemButton.name)
                    {
                        case "ButtonPlayer":
                            userCreateObject = Instantiate(PlayerFactory, Vector3.zero, Quaternion.identity);
                            break;

                        case "ButtonLight":
                            userCreateObject = Instantiate(LightFactory, Vector3.zero, Quaternion.identity);
                            break;

                        case "ButtonSwitch":
                            userCreateObject = Instantiate(SwitchFactory, Vector3.zero, Quaternion.identity);
                            break;

                        case "ButtonDoor":
                            userCreateObject = Instantiate(DoorFactory, Vector3.zero, Quaternion.identity);
                            break;

                        case "ButtonExit":
                            userCreateObject = Instantiate(ExitFactory, Vector3.zero, Quaternion.identity);
                            break;

                        case "ButtonWall":
                            userCreateObject = Instantiate(WallFactory, Vector3.zero, Quaternion.identity);
                            break;

                        default: // ȸ��, create, delete �� ��Ÿ �ٸ� ��ư�� ���ؼ��� �������� �ʴ´�.
                            break;
                    }
                }
                // ���콺�� �� ������ �̵��� ������ �����δ�.
                else if (userCreateObject != null)
                {
                    ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit))
                    {
                        // �����̴� ���콺 ��ġ�� ����� �̵���Ų��.
                        userCreateObject.transform.position = normalizePosition(hit.point.x, 0, hit.point.z);
                    }
                }
            }            
        }

        if (Input.GetMouseButtonUp(0))
        {
            userKeepedTarget = null; // �� ������ �־���, ������ �ʿ� �ִ� ������Ʈ
            userSelectedItemButton = null; // �� ������ �־���, item panel ���� ��ư

            // ���� item panel���� �巡���ؼ� ������Ʈ�� �� �󿡼� �����Ϸ��� �Ѵ�.
            if (userCreateObject != null)
            {
                // ���� ���콺 ��ġ�� �� ���� ���� �ʰ�, UI ���� �ִ�.
                if (EventSystem.current.IsPointerOverGameObject()) { Destroy(userCreateObject); } // �������� �ʰ� �����Ѵ�.
                userSelectedItemButton = null;
                userCreateObject = null;
            }
            // ����ڰ� �� �󿡼� ������Ʈ�� �����ߴ�.
            else if (userSelectedTarget != null && !EventSystem.current.IsPointerOverGameObject())
            {
                activateColorPanel();
            }
            
        }
    }

    // Color panel�� Ȱ��ȭ�ϰ�, ������ ������Ʈ�� ������ �ҷ��´�.
    private void activateColorPanel()
    {
        GameObject slider = ColorPanel.transform.GetChild(1).gameObject;
        GameObject toggle = ColorPanel.transform.GetChild(2).gameObject;
        Color targetColor;

        switch (userSelectedTarget.tag)
        {
            // Slider�� Ȱ��ȭ�ϰ�, Toggle�� ��Ȱ��ȭ�Ѵ�.
            case "Player":
                slider.SetActive(true);
                toggle.SetActive(false);

                targetColor = userSelectedTarget.transform.GetChild(1).GetComponent<Renderer>().material.color;
                Debug.Log(targetColor);
                slider.transform.GetChild(0).GetComponent<Slider>().value = targetColor.r * 255;
                slider.transform.GetChild(1).GetComponent<Slider>().value = targetColor.g * 255;
                slider.transform.GetChild(2).GetComponent<Slider>().value = targetColor.b * 255;
                slider.transform.GetChild(3).GetComponent<Slider>().value = targetColor.a * 255;
                break;

            case "Door":
            case "Exit":
            case "Wall":
                slider.SetActive(true);
                toggle.SetActive(false);

                targetColor = userSelectedTarget.GetComponent<Renderer>().material.color;
                slider.transform.GetChild(0).GetComponent<Slider>().value = targetColor.r * 255;
                slider.transform.GetChild(1).GetComponent<Slider>().value = targetColor.g * 255;
                slider.transform.GetChild(2).GetComponent<Slider>().value = targetColor.b * 255;
                slider.transform.GetChild(3).GetComponent<Slider>().value = targetColor.a * 255;
                break;

            // Slider�� ��Ȱ��ȭ�ϰ�, Toggle�� Ȱ��ȭ�Ѵ�.
            case "Torch":
                slider.SetActive(false);
                toggle.SetActive(true);

                TorchController torchScript = userSelectedTarget.GetComponent<TorchController>();
                toggle.transform.GetChild(0).GetComponent<Toggle>().isOn = torchScript.red;
                toggle.transform.GetChild(1).GetComponent<Toggle>().isOn = torchScript.green;
                toggle.transform.GetChild(2).GetComponent<Toggle>().isOn = torchScript.blue;
                break;

            case "Switch":
                slider.SetActive(false);
                toggle.SetActive(true);

                TorchSwitchController switchScript = userSelectedTarget.GetComponent<TorchSwitchController>();
                toggle.transform.GetChild(0).GetComponent<Toggle>().isOn = switchScript.red;
                toggle.transform.GetChild(1).GetComponent<Toggle>().isOn = switchScript.green;
                toggle.transform.GetChild(2).GetComponent<Toggle>().isOn = switchScript.blue;
                break;
        }
    }

    // ��ġ�� 0, 2, 4, ... ������ �����Ѵ�.
    private float normalizeFloat(float num)
    {
        return (1 + (int) Math.Abs(num)) / 2 * 2 * Math.Sign(num);
    }
    private Vector3 normalizePosition(float x, float y, float z)
    {
        return new Vector3(normalizeFloat(x), normalizeFloat(y), normalizeFloat(z));
    }
    private Vector3 normalizePosition(Vector3 prevPos)
    {
        return new Vector3(normalizeFloat(prevPos.x), normalizeFloat(prevPos.y), normalizeFloat(prevPos.z));
    }
}
