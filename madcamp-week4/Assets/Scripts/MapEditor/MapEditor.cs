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
    GameObject userKeepedItemButton = null;

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

    // �� ���� ������Ʈ Ŭ�� �� �����ϴ� �г�
    public GameObject ColorPanel;
    public GameObject RotatePanel;

    public GameObject itemPanel;

    // ���� ī�޶� ��ġ
    float cameraPosX;
    float cameraPosY;
    float cameraPosZ;
    public float mouseSpeed = 0.1f;
    public float wheelSpeed = 20;

    // �� �� �󿡼� �����ϴ� ������Ʈ���� json ������ �ƴ� GameObject �������� �ӽ÷� �����Ѵ�.
    public static GameObject[] gameObjects = new GameObject[100];
    public static int mapWidth = 10;
    public static int mapHeight = 10;


    // Start is called before the first frame update
    void Start()
    {
        cameraPosX = Camera.main.transform.position.x;
        cameraPosY = Camera.main.transform.position.y;
        cameraPosZ = Camera.main.transform.position.z;
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

                // ���콺 Ŭ���� ȭ�� ��ǥ�� ����ĳ��Ʈ�� �����Ͽ� 3D ���� ��ǥ�� ���
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit)) // ����ĳ��Ʈ�� �ƹ� ��ü�� �ε��� ���
                {
                    Debug.Log("�ε��� 3D ���� ��ǥ: " + hit.point);

                    // (1) ���� create ����̰�, item ��ư�� �����ߴ�.
                    if (CreateButton.isPressed && ItemButton.selectedItemButton != null)
                    {
                        // �ٴ��� ��쿡�� ������ item�� �����Ѵ�.
                        if (hit.collider.gameObject.CompareTag("Plane"))
                        {
                            Vector3 mousePos = NormalizePosition(hit.point.x, 0, hit.point.z);
                            Color newColor = ColorPanel.transform.GetChild(0).GetComponent<Image>().color;

                            // �ش� ��ġ�� �ٸ� ������Ʈ�� �̹� �ִ��� Ȯ���ϰ�, ������ �����Ѵ�.
                            CheckClickInstantiate(mousePos, newColor);
                        }
                    }

                    // (2) ���� delete ����̰�, Ŭ���� ����� �ٴ��� �ƴ� ������Ʈ�̴�.
                    else if (DeleteButton.isPressed && !hit.collider.gameObject.CompareTag("Plane"))
                    {
                        Destroy(hit.collider.gameObject);

                        // ����Ʈ���� �ش� ������Ʈ�� �����Ѵ�.
                        int index = CalculateIndex(hit.collider.gameObject.transform.position);
                        gameObjects[index] = null;
                    }

                    // (3) create�� delete�� �ƴ�, �Ϲ����� ��Ȳ
                    else
                    {
                        // ���� item panel���� Ŭ���ߴ� ��ư�� �־�����, �ش� ��ư�� �����Ѵ�.
                        if (ItemButton.selectedItemButton != null) itemPanel.GetComponent<ItemButton>().OnClickItemButton(null);

                        // ���� Ŭ���� ����� �ٴ��� ��쿡�� ���� ����� �ʱ�ȭ�Ѵ�.
                        if (hit.collider.gameObject.CompareTag("Plane"))
                        {
                            userSelectedTarget = null;

                            ColorPanel.transform.GetChild(1).gameObject.SetActive(false);
                            ColorPanel.transform.GetChild(2).gameObject.SetActive(false);
                            RotatePanel.SetActive(false);
                        }
                        else // �׷��� ���� ��� ����� �����Ѵ�. (����, �̵�, ���� ��� Ȱ��ȭ)
                        {
                            userSelectedTarget = hit.collider.gameObject;
                            userKeepedTarget = hit.collider.gameObject;
                        } 
                    }
                }
            }
            // UI ��Ҹ� �����ߴ�. (�������� �巡�� �� ������� instantiate�� �� �ְ� �Ѵ�)
            else
            {
                // item panel ������ Ŭ���� ��ư�� ����Ų��.
                userKeepedItemButton = EventSystem.current.currentSelectedGameObject;
                if (userKeepedItemButton != null) Debug.Log(userKeepedItemButton.name);
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
                    // �����̴� ���콺 ��ġ�� ������Ʈ�� ������, �ش� ��ġ�� ����� �̵���Ų��.
                    Vector3 newPos = NormalizePosition(hit.point.x, 0, hit.point.z);
                    int newIndex = CalculateIndex(newPos);
                    if (gameObjects[newIndex] == null)
                    {
                        int prevIndex = CalculateIndex(userKeepedTarget.transform.position);
                        gameObjects[prevIndex] = null; // ���� �� ����

                        userKeepedTarget.transform.position = newPos; // ���̴� �� ����
                        gameObjects[newIndex] = userKeepedTarget; // ���Ӱ� ����
                    }
                }
            }
            // item panel ������ �������� Ŭ���� ������ ���콺�� ��� ������ �ִ�.
            else if (userKeepedItemButton != null)
            {
                // ���콺�� �� ���� ���·� ���콺 ��ġ�� �� ���� ���ʷ� �̵��ߴ�.
                if (userCreateObject == null && !EventSystem.current.IsPointerOverGameObject())
                {
                    switch (userKeepedItemButton.name)
                    {
                        case "ButtonPlayer":
                            userCreateObject = Instantiate(PlayerFactory, Vector3.zero, Quaternion.identity);
                            break;

                        case "ButtonSword":
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
                        userCreateObject.transform.position = NormalizePosition(hit.point.x, 0, hit.point.z);
                    }
                }
            }            
        }

        if (Input.GetMouseButtonUp(0))
        {
            userKeepedTarget = null; // �� ������ �־���, ������ �ʿ� �ִ� ������Ʈ
            userKeepedItemButton = null; // �� ������ �־���, item panel ���� ��ư

            // ���� item panel���� �巡���ؼ� ������Ʈ�� �� �󿡼� �����Ϸ��� �Ѵ�.
            if (userCreateObject != null)
            {
                CheckDragInstantiate();
            }
            // ����ڰ� �� �󿡼� ������Ʈ�� �����ߴ�.
            else if (userSelectedTarget != null && !EventSystem.current.IsPointerOverGameObject())
            {
                ActivateColorPanel();
                RotatePanel.SetActive(true);
            }
            
        }

        // ���콺 ������ ��ư�� ������ ������, ȭ���� �̵��� �� �ִ�.
        if (Input.GetMouseButton(1))
        {
            // �ָ� ��������� �� ���� �����̰�, ������ ��������� �� õõ�� �����δ�.
            cameraPosX -= Input.GetAxis("Mouse X") * mouseSpeed * Camera.main.orthographicSize;
            cameraPosZ -= Input.GetAxis("Mouse Y") * mouseSpeed * Camera.main.orthographicSize;

            Camera.main.transform.position = new Vector3(cameraPosX, cameraPosY, cameraPosZ);
        }
        // ���콺 �ٷ� Ȯ�� ��Ҹ� �� �� �ִ�.
        Camera.main.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * wheelSpeed;
        if (Camera.main.orthographicSize < 1) Camera.main.orthographicSize = 1;
    }




    // ������Ʈ�� instantiate�ϱ� ����, �ش� ��ġ�� �ٸ� ������Ʈ�� �̹� �ִ����� json���� Ȯ���Ѵ�.
    private void CheckClickInstantiate(Vector3 mousePos, Color newColor)
    {
        int index = CalculateIndex(mousePos);

        // �ش� ��ġ�� �ٸ� ������Ʈ�� �̹� �ִٸ�, �������� �ʴ´�.
        if (gameObjects[index] != null) return;
        
        switch (ItemButton.selectedItemButton.name)
        {
            case "ButtonPlayer":
                userCreateObject = Instantiate(PlayerFactory, mousePos, Quaternion.identity);
                userCreateObject.transform.GetChild(1).GetComponent<Renderer>().material.color = newColor; // Alpha_Surface ���� ����
                break;

            case "ButtonSword":
                userCreateObject = Instantiate(LightFactory, mousePos, Quaternion.identity);
                TorchController lightScript = userCreateObject.GetComponent<TorchController>();

                if (newColor.r == 1) lightScript.red = true;
                else lightScript.red = false;

                if (newColor.g == 1) lightScript.green = true;
                else lightScript.green = false;

                if (newColor.b == 1) lightScript.blue = true;
                else lightScript.blue = false;
                break;

            case "ButtonSwitch":
                userCreateObject = Instantiate(SwitchFactory, mousePos, Quaternion.identity);
                TorchSwitchController switchScript = userCreateObject.GetComponent<TorchSwitchController>();

                if (newColor.r == 1) switchScript.red = true;
                else switchScript.red = false;

                if (newColor.g == 1) switchScript.green = true;
                else switchScript.green = false;

                if (newColor.b == 1) switchScript.blue = true;
                else switchScript.blue = false;
                break;

            case "ButtonDoor":
                userCreateObject = Instantiate(DoorFactory, mousePos, Quaternion.identity);
                userCreateObject.GetComponent<Renderer>().material.color = newColor;
                break;

            case "ButtonExit":
                userCreateObject = Instantiate(ExitFactory, mousePos, Quaternion.identity);
                userCreateObject.GetComponent<Renderer>().material.color = newColor;
                break;

            case "ButtonWall":
                userCreateObject = Instantiate(WallFactory, mousePos, Quaternion.identity);
                userCreateObject.GetComponent<Renderer>().material.color = newColor;
                break;

            default: // ȸ��, create, delete �� ��Ÿ �ٸ� ��ư�� ���ؼ��� �������� �ʴ´�.
                break;
        }
        gameObjects[index] = userCreateObject;
        userCreateObject = null;
    }

    // �巡���ؼ� �����Ϸ��� �Ѵ�.
    private void CheckDragInstantiate()
    {
        int index = CalculateIndex(userCreateObject.transform.position);

        // ���� ���� ���콺 ��ġ�� �� ���� ���� �ʰ� UI ���� �ְų�, �ش� ��ġ�� �ٸ� ������Ʈ�� �̹� ������, �������� �ʰ� �����Ѵ�.
        if (EventSystem.current.IsPointerOverGameObject() || gameObjects[index] != null) Destroy(userCreateObject);

        // �׷��� �ʴٸ�, ������ ����� gameObjects�� �ݿ��Ѵ�.
        else gameObjects[index] = userCreateObject;

        userKeepedItemButton = null;
        userCreateObject = null;
    }

    public int CalculateIndex(Vector3 pos)
    {
        return (int)(pos.x + mapWidth * pos.z) / 2;
    }



    // Color panel�� Ȱ��ȭ�ϰ�, ������ ������Ʈ�� ������ �ҷ��´�.
    private void ActivateColorPanel()
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
            case "Sword":
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
    private float NormalizeFloat(float num)
    {
        return (1 + (int) Math.Abs(num)) / 2 * 2 * Math.Sign(num);
    }
    private Vector3 NormalizePosition(float x, float y, float z)
    {
        return new Vector3(NormalizeFloat(x), NormalizeFloat(y), NormalizeFloat(z));
    }
    private Vector3 NormalizePosition(Vector3 prevPos)
    {
        return new Vector3(NormalizeFloat(prevPos.x), NormalizeFloat(prevPos.y), NormalizeFloat(prevPos.z));
    }
}
