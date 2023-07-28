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
    int userKeepedTargetPrevIndex; // �巡�� �ϱ� ��
    int userKeepedTargetNewIndex; // �巡�� ���� ��

    // ����ڰ� item panel���� ���콺�� ��� ������ �ִ� ������Ʈ (UI�� item ��ư�� ����Ų��)
    GameObject userKeepedItemButton = null;

    // ����ڰ� item panel���� ���콺�� �������� �ʿ� ������ ������ ������Ʈ (������ ������ ������Ʈ�� ����Ų��) (factory���� instantiate�Ѵ�)
    GameObject userCreateObject = null;

    public GameObject createButton;
    public GameObject deleteButton;

    // ���콺 ��ġ Ʈ��ŷ��
    Ray ray;

    // �巡�� �� ������� create�� �� �ʿ��ϴ�.
    public GameObject PlayerFactory;
    public GameObject LightFactory;
    public GameObject SwitchFactory;
    public GameObject DoorFactory;
    public GameObject ExitFactory;
    public GameObject WallFactory;
    public GameObject HighlightSwitchFactory;

    // �� ���� ������Ʈ Ŭ�� �� �����ϴ� �г�
    public GameObject ColorPanel;
    public GameObject RotatePanel;
    public GameObject itemPanel;
    public GameObject SizePanel;
    public GameObject CreatePanel;

    // ���� ����ġ �����ϱ�
    public GameObject ConnectSwitchToast;
    public static bool isSelectingSwitch = false;
    GameObject selectedDoor;
    Dictionary<TorchSwitchController, GameObject> highlight; // ������ ���� ����Ǿ� �ִ� ����ġ���� �����ϱ� ���� GameObject
    Dictionary<GameObject, List<GameObject>> switchToDoor; // ������ ����ġ�� ����Ǿ� �ִ� �� ����Ʈ

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

        isSelectingSwitch = false;
        highlight = new Dictionary<TorchSwitchController, GameObject>();
        switchToDoor = new Dictionary<GameObject, List<GameObject>>();
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
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit)) // ����ĳ��Ʈ�� �ƹ� ��ü�� �ε��� ���
                {
                    Debug.Log("�ε��� 3D ���� ��ǥ: " + hit.point);
                    int index = CalculateIndex(NormalizePosition(hit.point, ""));

                    // (1) ���� create ����̰�, item ��ư�� �����ߴ�.
                    // Ŭ���� ��ġ�� �ٸ� ������Ʈ�� ���ٸ�, ������ item�� �����Ѵ�.
                    if (createButton.GetComponent<CreateButton>().isPressed && ItemButton.selectedItemButton != null && gameObjects[index] == null)
                    {
                        Vector3 mousePos = NormalizePosition(hit.point.x, 0, hit.point.z, ItemButton.selectedItemButton.name);
                        Color newColor = ColorPanel.transform.GetChild(0).GetComponent<Image>().color;

                        ClickInstantiate(mousePos, newColor);
                    }

                    // (2) ���� delete ����̰�, Ŭ���� ��ġ�� ������Ʈ�� �ִ�..
                    else if (deleteButton.GetComponent<DeleteButton>().isPressed && gameObjects[index] != null)
                    {
                        Destroy(gameObjects[index]);

                        // ����Ʈ���� �ش� ������Ʈ�� �����Ѵ�.
                        gameObjects[index] = null;
                    }

                    // (3) create�� delete�� �ƴ�, �Ϲ����� ��Ȳ
                    else
                    {
                        // ���� item panel���� Ŭ���ߴ� ��ư�� �־�����, �ش� ��ư�� �����Ѵ�.
                        if (ItemButton.selectedItemButton != null) itemPanel.GetComponent<ItemButton>().OnClickItemButton(null);

                        // ���� Ŭ���� ����� �ٴ��� ��쿡�� ���� ����� �ʱ�ȭ�Ѵ�.
                        if (gameObjects[index] == null)
                        {
                            userSelectedTarget = null;

                            ColorPanel.transform.GetChild(1).gameObject.SetActive(false);
                            ColorPanel.transform.GetChild(2).gameObject.SetActive(false);
                            RotatePanel.SetActive(false);
                        }
                        // ���� ���� ������ ����ġ���� ���� ���̴�.
                        else if (isSelectingSwitch && gameObjects[index].tag == "Switch")
                        {
                            GameObject selectedSwitch = gameObjects[index];
                            TorchSwitchController switchController = selectedSwitch.GetComponent<TorchSwitchController>();
                            List<TorchSwitchController> switchList = selectedDoor.GetComponent<DoorController>().switchControllers;

                            if (switchList.Contains(switchController)) // ������ �����߾��� ����ġ�̴�.
                            {
                                switchList.Remove(switchController);
                                GameObject hl = highlight[switchController];
                                Destroy(hl);
                                highlight.Remove(switchController);

                                List<GameObject> result;
                                if (switchToDoor.TryGetValue(selectedSwitch, out result))
                                {
                                    result.Remove(selectedDoor);
                                }
                                else
                                {
                                    Debug.Log("error?");
                                }
                            }
                            else // �̹��� ���Ӱ� ������ ����ġ�̴�.
                            {
                                switchList.Add(switchController);
                                Vector3 switchPos = gameObjects[index].transform.position;
                                GameObject hl = Instantiate(HighlightSwitchFactory, new Vector3(switchPos.x + 0.7f, switchPos.y, switchPos.z + 0.7f), Quaternion.identity);
                                highlight.Add(switchController, hl);

                                List<GameObject> result;
                                if (switchToDoor.TryGetValue(selectedSwitch, out result))
                                {
                                    result.Add(selectedDoor);
                                }
                                else
                                {
                                    List<GameObject> connectedDoor = new List<GameObject> {selectedDoor};
                                    switchToDoor.Add(selectedSwitch, connectedDoor);
                                }
                            }
                        }
                        else // �׷��� ���� ��� ����� �����Ѵ�. (����, �̵�, ���� ��� Ȱ��ȭ)
                        {
                            userSelectedTarget = gameObjects[index];
                            userKeepedTarget = gameObjects[index];
                            userKeepedTargetPrevIndex = index;
                            userKeepedTargetNewIndex = index;

                            // ���� Ŭ���� ���, ������ ����ġ�� �����ϴ� â�� ����.
                            if (gameObjects[index].tag == "Door")
                            {
                                ConnectSwitchToast.SetActive(true);
                                isSelectingSwitch = true;
                                selectedDoor = gameObjects[index];

                                // ���� ����Ǿ� �ִ� ����ġ���� �����ؼ� ��Ÿ����.
                                List<TorchSwitchController> switchList = selectedDoor.GetComponent<DoorController>().switchControllers;
                                foreach (TorchSwitchController switchController in switchList)
                                {
                                    if (switchController != null)
                                    {
                                        Vector3 switchPos = switchController.gameObject.transform.position;
                                        GameObject hl = Instantiate(HighlightSwitchFactory, new Vector3(switchPos.x + 0.7f, switchPos.y, switchPos.z + 0.7f), Quaternion.identity);
                                        highlight.Add(switchController, hl);
                                    }
                                }
                                SizePanel.SetActive(false);
                                CreatePanel.SetActive(false);
                            }
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
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    // �����̴� ���콺 ��ġ�� ������Ʈ�� ������, �ش� ��ġ�� ����� �̵���Ų��.
                    Vector3 newPos = NormalizePosition(hit.point.x, 0, hit.point.z, userKeepedTarget.tag);
                    int newIndex = CalculateIndex(newPos);
                    if (gameObjects[newIndex] == null)
                    {
                        if (userKeepedTarget.tag == "Door") userKeepedTarget.GetComponent<DoorController>().closedPosition = newPos;
                        userKeepedTarget.transform.position = newPos; // ���̴� �� ����
                        userKeepedTargetNewIndex = newIndex;
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
                            userCreateObject = Instantiate(PlayerFactory, new Vector3(-1, -1, -1), Quaternion.identity);
                            userCreateObject.transform.GetChild(0).gameObject.SetActive(false); // ī�޶� ����
                            userCreateObject.GetComponent<PlayerController>().enabled = false;
                            break;

                        case "ButtonTorch":
                            userCreateObject = Instantiate(LightFactory, new Vector3(-1, -1, -1), Quaternion.identity);
                            break;

                        case "ButtonSwitch":
                            userCreateObject = Instantiate(SwitchFactory, new Vector3(-1, -1, -1), Quaternion.identity);
                            break;

                        case "ButtonDoor":
                            userCreateObject = Instantiate(DoorFactory, new Vector3(-1, -1, -1), Quaternion.identity);
                            break;

                        case "ButtonExit":
                            userCreateObject = Instantiate(ExitFactory, new Vector3(-1, -1, -1), Quaternion.identity);
                            break;

                        case "ButtonWall":
                            userCreateObject = Instantiate(WallFactory, new Vector3(-1, -1, -1), Quaternion.identity);
                            break;

                        default: // ȸ��, create, delete �� ��Ÿ �ٸ� ��ư�� ���ؼ��� �������� �ʴ´�.
                            break;
                    }
                }
                // ���콺�� �� ������ �̵��� ������ �����δ�.
                else if (userCreateObject != null)
                {
                    ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit))
                    {
                        // �����̴� ���콺 ��ġ�� ����� �̵���Ų��.
                        int index = CalculateIndex(NormalizePosition(hit.point.x, 0, hit.point.z, userCreateObject.tag));
                        if (gameObjects[index] == null)
                        {
                            if (userCreateObject.tag == "Door") userCreateObject.GetComponent<DoorController>().closedPosition = NormalizePosition(hit.point.x, 0, hit.point.z, "Door");
                            userCreateObject.transform.position = NormalizePosition(hit.point.x, 0, hit.point.z, userCreateObject.tag);
                        }
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            // ���� �� �󿡼� ������Ʈ�� �� ������ �̵��ϰ� �ִ�.
            if (userKeepedTarget != null && userKeepedTargetPrevIndex != userKeepedTargetNewIndex)
            {
                gameObjects[userKeepedTargetPrevIndex] = null;
                gameObjects[userKeepedTargetNewIndex] = userKeepedTarget;
            }

            // ���� item panel���� �巡���ؼ� ������Ʈ�� �� �󿡼� �����Ϸ��� �Ѵ�.
            else if (userCreateObject != null)
            {
                CheckDragInstantiate();
            }
            // ����ڰ� �� �󿡼� ������Ʈ�� �����ߴ�.
            else if (userSelectedTarget != null && !EventSystem.current.IsPointerOverGameObject())
            {
                ActivateColorPanel();
                RotatePanel.SetActive(true);
            }
            userKeepedTarget = null; // �� ������ �־���, ������ �ʿ� �ִ� ������Ʈ
            userKeepedItemButton = null; // �� ������ �־���, item panel ���� ��ư

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
    private void ClickInstantiate(Vector3 mousePos, Color newColor)
    {
        Toggle transparentToggle = ColorPanel.transform.GetChild(1).gameObject.transform.GetChild(4).gameObject.GetComponent<Toggle>();

        switch (ItemButton.selectedItemButton.name)
        {
            case "ButtonPlayer":
                userCreateObject = Instantiate(PlayerFactory, mousePos, Quaternion.identity);
                userCreateObject.transform.GetChild(2).GetComponent<Renderer>().material.color = newColor; // Alpha_Surface ���� ����
                userCreateObject.transform.GetChild(0).gameObject.SetActive(false); // ī�޶� ����
                userCreateObject.GetComponent<PlayerController>().enabled = false;
                break;

            case "ButtonTorch":
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
                userCreateObject.GetComponent<DoorController>().closedPosition = mousePos;
                userCreateObject.GetComponent<DoorController>().isTransparent = transparentToggle.isOn;
                userCreateObject.GetComponent<DoorController>().applyProperty();
                break;

            case "ButtonExit":
                userCreateObject = Instantiate(ExitFactory, mousePos, Quaternion.identity);
                // userCreateObject.GetComponent<Renderer>().material.color = newColor;
                break;

            case "ButtonWall":
                userCreateObject = Instantiate(WallFactory, mousePos, Quaternion.identity);
                userCreateObject.GetComponent<Renderer>().material.color = newColor;
                userCreateObject.GetComponent<WallController>().isTransparent = transparentToggle.isOn;
                userCreateObject.GetComponent<WallController>().applyProperty();
                break;

            default: // ȸ��, create, delete �� ��Ÿ �ٸ� ��ư�� ���ؼ��� �������� �ʴ´�.
                break;
        }
        int index = CalculateIndex(mousePos);
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

    // Door�� Ŭ������ ��, � ����ġ���� ������ �������� ��Ÿ����.


    // ConnectSwitchToast���� Done ��ư�� ������, �г��� �ݴ´�.
    public void OnClickDoneButton()
    {
        ConnectSwitchToast.SetActive(false);
        isSelectingSwitch = false;
        selectedDoor = null;
        foreach(GameObject hl in highlight.Values) 
        {
            if (hl != null) Destroy(hl);
        }
        highlight.Clear();

        SizePanel.SetActive(true);
        CreatePanel.SetActive(true);
    }


    // Color panel�� Ȱ��ȭ�ϰ�, ������ ������Ʈ�� ������ �ҷ��´�.
    private void ActivateColorPanel()
    {
        GameObject slider = ColorPanel.transform.GetChild(1).gameObject;
        GameObject toggle = ColorPanel.transform.GetChild(2).gameObject;
        GameObject transparent = slider.transform.GetChild(4).gameObject;
        Color targetColor;

        switch (userSelectedTarget.tag)
        {
            // Slider�� Ȱ��ȭ�ϰ�, Toggle�� ��Ȱ��ȭ�Ѵ�.
            case "Player":
                slider.SetActive(true);
                toggle.SetActive(false);
                transparent.SetActive(false);

                targetColor = userSelectedTarget.transform.GetChild(2).GetComponent<Renderer>().material.color;

                slider.transform.GetChild(0).GetComponent<Slider>().value = targetColor.r * 255;
                slider.transform.GetChild(1).GetComponent<Slider>().value = targetColor.g * 255;
                slider.transform.GetChild(2).GetComponent<Slider>().value = targetColor.b * 255;
                slider.transform.GetChild(3).GetComponent<Slider>().value = targetColor.a * 255;
                break;

            case "Door":
                slider.SetActive(true);
                toggle.SetActive(false);
                transparent.SetActive(true);

                transparent.GetComponent<Toggle>().isOn = userSelectedTarget.GetComponent<DoorController>().isTransparent;
                targetColor = userSelectedTarget.GetComponent<Renderer>().material.color;
                slider.transform.GetChild(0).GetComponent<Slider>().value = targetColor.r * 255;
                slider.transform.GetChild(1).GetComponent<Slider>().value = targetColor.g * 255;
                slider.transform.GetChild(2).GetComponent<Slider>().value = targetColor.b * 255;
                slider.transform.GetChild(3).GetComponent<Slider>().value = targetColor.a * 255;
                break;

            // case "Exit":
            case "Wall":
                slider.SetActive(true);
                toggle.SetActive(false);
                transparent.SetActive(true);

                transparent.GetComponent<Toggle>().isOn = userSelectedTarget.GetComponent<WallController>().isTransparent;
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
                transparent.SetActive(false);

                TorchController torchScript = userSelectedTarget.GetComponent<TorchController>();
                toggle.transform.GetChild(0).GetComponent<Toggle>().isOn = torchScript.red;
                toggle.transform.GetChild(1).GetComponent<Toggle>().isOn = torchScript.green;
                toggle.transform.GetChild(2).GetComponent<Toggle>().isOn = torchScript.blue;
                break;

            case "Switch":
                slider.SetActive(false);
                toggle.SetActive(true);
                transparent.SetActive(false);

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
    private Vector3 NormalizePosition(float x, float y, float z, string type)
    {
        float newY = 0f;
        if (type == "Switch" || type == "Door" || type == "Wall" || type == "ButtonSwitch" || type == "ButtonDoor" || type == "ButtonWall") newY = 1f;

        return new Vector3(NormalizeFloat(x), newY, NormalizeFloat(z));
    }
    private Vector3 NormalizePosition(Vector3 prevPos, string type)
    {
        float newY = 0f;
        if (type == "Switch" || type == "Door" || type == "Wall" || type == "ButtonSwitch" || type == "ButtonDoor" || type == "ButtonWall") newY = 1f;

        return new Vector3(NormalizeFloat(prevPos.x), newY, NormalizeFloat(prevPos.z));
    }
}
