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
    // 사용자가 맵에서 선택한 오브젝트 (회전 및 삭제 기능) (한 번 클릭하면 된다)
    public static GameObject userSelectedTarget = null;

    // 사용자가 맵에서 마우스로 계속 누르고 있는 오브젝트 (이동 기능) (꾹 누르고 있어야 한다)
    GameObject userKeepedTarget = null;

    // 사용자가 item panel에서 마우스로 계속 누르고 있는 오브젝트 (UI의 item 버튼을 가리킨다)
    GameObject userKeepedItemButton = null;

    // 사용자가 item panel에서 마우스로 움직여서 맵에 실제로 생성할 오브젝트 (실제로 생성할 오브젝트를 가리킨다) (factory에서 instantiate한다)
    GameObject userCreateObject = null;

    public GameObject deleteButton;

    // 마우스 위치 트래킹용
    Ray ray;
    RaycastHit hit;

    // 드래그 앤 드랍으로 create할 때 필요하다.
    public GameObject PlayerFactory;
    public GameObject LightFactory;
    public GameObject SwitchFactory;
    public GameObject DoorFactory;
    public GameObject ExitFactory;
    public GameObject WallFactory;

    // 맵 상의 오브젝트 클릭 시 수정하는 패널
    public GameObject ColorPanel;
    public GameObject RotatePanel;

    public GameObject itemPanel;

    // 현재 카메라 위치
    float cameraPosX;
    float cameraPosY;
    float cameraPosZ;
    public float mouseSpeed = 0.1f;
    public float wheelSpeed = 20;

    // 이 맵 상에서 존재하는 오브젝트들을 json 형식이 아닌 GameObject 형식으로 임시로 저장한다.
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
        // 마우스 왼쪽 버튼 클릭을 감지
        if (Input.GetMouseButtonDown(0))
        {
            // if문을 만족시키면, UI 요소 위가 아니다. (맵 위다)
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("UI 위가 아니다.");

                // 마우스 클릭한 화면 좌표를 레이캐스트로 변경하여 3D 월드 좌표로 얻기
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit)) // 레이캐스트가 아무 물체에 부딪힌 경우
                {
                    Debug.Log("부딪힌 3D 월드 좌표: " + hit.point);

                    // (1) 현재 create 모드이고, item 버튼을 선택했다.
                    if (CreateButton.isPressed && ItemButton.selectedItemButton != null)
                    {
                        // 바닥인 경우에는 선택한 item을 생성한다.
                        if (hit.collider.gameObject.CompareTag("Plane"))
                        {
                            Vector3 mousePos = NormalizePosition(hit.point.x, 0, hit.point.z);
                            Color newColor = ColorPanel.transform.GetChild(0).GetComponent<Image>().color;

                            // 해당 위치에 다른 오브젝트가 이미 있는지 확인하고, 없으면 생성한다.
                            CheckClickInstantiate(mousePos, newColor);
                        }
                    }

                    // (2) 현재 delete 모드이고, 클릭한 대상이 바닥이 아닌 오브젝트이다.
                    else if (DeleteButton.isPressed && !hit.collider.gameObject.CompareTag("Plane"))
                    {
                        Destroy(hit.collider.gameObject);

                        // 리스트에서 해당 오브젝트를 삭제한다.
                        int index = CalculateIndex(hit.collider.gameObject.transform.position);
                        gameObjects[index] = null;
                    }

                    // (3) create와 delete가 아닌, 일반적인 상황
                    else
                    {
                        // 만약 item panel에서 클릭했던 버튼이 있었으면, 해당 버튼을 해제한다.
                        if (ItemButton.selectedItemButton != null) itemPanel.GetComponent<ItemButton>().OnClickItemButton(null);

                        // 만약 클릭한 대상이 바닥인 경우에는 선택 대상을 초기화한다.
                        if (hit.collider.gameObject.CompareTag("Plane"))
                        {
                            userSelectedTarget = null;

                            ColorPanel.transform.GetChild(1).gameObject.SetActive(false);
                            ColorPanel.transform.GetChild(2).gameObject.SetActive(false);
                            RotatePanel.SetActive(false);
                        }
                        else // 그렇지 않은 경우 대상을 선택한다. (수정, 이동, 삭제 등등 활성화)
                        {
                            userSelectedTarget = hit.collider.gameObject;
                            userKeepedTarget = hit.collider.gameObject;
                        } 
                    }
                }
            }
            // UI 요소를 선택했다. (아이템을 드래그 앤 드랍으로 instantiate할 수 있게 한다)
            else
            {
                // item panel 위에서 클릭한 버튼을 가리킨다.
                userKeepedItemButton = EventSystem.current.currentSelectedGameObject;
                if (userKeepedItemButton != null) Debug.Log(userKeepedItemButton.name);
            } 
        }

        if (Input.GetMouseButton(0)) 
        {
            // 맵 위에서 대상을 클릭한 다음에 마우스로 계속 누르고 있다.
            if (userKeepedTarget != null)
            {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    // 움직이는 마우스 위치에 오브젝트가 없으면, 해당 위치로 대상을 이동시킨다.
                    Vector3 newPos = NormalizePosition(hit.point.x, 0, hit.point.z);
                    int newIndex = CalculateIndex(newPos);
                    if (gameObjects[newIndex] == null)
                    {
                        int prevIndex = CalculateIndex(userKeepedTarget.transform.position);
                        gameObjects[prevIndex] = null; // 이전 꺼 삭제

                        userKeepedTarget.transform.position = newPos; // 보이는 것 수정
                        gameObjects[newIndex] = userKeepedTarget; // 새롭게 저장
                    }
                }
            }
            // item panel 위에서 아이템을 클릭한 다음에 마우스로 계속 누르고 있다.
            else if (userKeepedItemButton != null)
            {
                // 마우스를 꾹 누른 상태로 마우스 위치를 맵 위로 최초로 이동했다.
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

                        default: // 회전, create, delete 등 기타 다른 버튼에 대해서는 동작하지 않는다.
                            break;
                    }
                }
                // 마우스를 맵 밖으로 이동한 다음에 움직인다.
                else if (userCreateObject != null)
                {
                    ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit))
                    {
                        // 움직이는 마우스 위치로 대상을 이동시킨다.
                        userCreateObject.transform.position = NormalizePosition(hit.point.x, 0, hit.point.z);
                    }
                }
            }            
        }

        if (Input.GetMouseButtonUp(0))
        {
            userKeepedTarget = null; // 꾹 누르고 있었던, 기존에 맵에 있던 오브젝트
            userKeepedItemButton = null; // 꾹 누르고 있었던, item panel 위의 버튼

            // 현재 item panel에서 드래그해서 오브젝트를 맵 상에서 생성하려고 한다.
            if (userCreateObject != null)
            {
                CheckDragInstantiate();
            }
            // 사용자가 맵 상에서 오브젝트를 선택했다.
            else if (userSelectedTarget != null && !EventSystem.current.IsPointerOverGameObject())
            {
                ActivateColorPanel();
                RotatePanel.SetActive(true);
            }
            
        }

        // 마우스 오른쪽 버튼을 누르고 있으면, 화면을 이동할 수 있다.
        if (Input.GetMouseButton(1))
        {
            // 멀리 축소했으면 더 빨리 움직이고, 가까이 축소했으면 더 천천히 움직인다.
            cameraPosX -= Input.GetAxis("Mouse X") * mouseSpeed * Camera.main.orthographicSize;
            cameraPosZ -= Input.GetAxis("Mouse Y") * mouseSpeed * Camera.main.orthographicSize;

            Camera.main.transform.position = new Vector3(cameraPosX, cameraPosY, cameraPosZ);
        }
        // 마우스 휠로 확대 축소를 할 수 있다.
        Camera.main.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * wheelSpeed;
        if (Camera.main.orthographicSize < 1) Camera.main.orthographicSize = 1;
    }




    // 오브젝트를 instantiate하기 전에, 해당 위치에 다른 오브젝트가 이미 있는지를 json에서 확인한다.
    private void CheckClickInstantiate(Vector3 mousePos, Color newColor)
    {
        int index = CalculateIndex(mousePos);

        // 해당 위치에 다른 오브젝트가 이미 있다면, 생성하지 않는다.
        if (gameObjects[index] != null) return;
        
        switch (ItemButton.selectedItemButton.name)
        {
            case "ButtonPlayer":
                userCreateObject = Instantiate(PlayerFactory, mousePos, Quaternion.identity);
                userCreateObject.transform.GetChild(1).GetComponent<Renderer>().material.color = newColor; // Alpha_Surface 색상 변경
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

            default: // 회전, create, delete 등 기타 다른 버튼에 대해서는 동작하지 않는다.
                break;
        }
        gameObjects[index] = userCreateObject;
        userCreateObject = null;
    }

    // 드래그해서 생성하려고 한다.
    private void CheckDragInstantiate()
    {
        int index = CalculateIndex(userCreateObject.transform.position);

        // 만약 현재 마우스 위치가 맵 위에 있지 않고 UI 위에 있거나, 해당 위치에 다른 오브젝트가 이미 있으면, 생성하지 않고 삭제한다.
        if (EventSystem.current.IsPointerOverGameObject() || gameObjects[index] != null) Destroy(userCreateObject);

        // 그렇지 않다면, 생성한 사실을 gameObjects에 반영한다.
        else gameObjects[index] = userCreateObject;

        userKeepedItemButton = null;
        userCreateObject = null;
    }

    public int CalculateIndex(Vector3 pos)
    {
        return (int)(pos.x + mapWidth * pos.z) / 2;
    }



    // Color panel을 활성화하고, 선택한 오브젝트의 색깔을 불러온다.
    private void ActivateColorPanel()
    {
        GameObject slider = ColorPanel.transform.GetChild(1).gameObject;
        GameObject toggle = ColorPanel.transform.GetChild(2).gameObject;
        Color targetColor;

        switch (userSelectedTarget.tag)
        {
            // Slider을 활성화하고, Toggle을 비활성화한다.
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

            // Slider을 비활성화하고, Toggle을 활성화한다.
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

    // 위치를 0, 2, 4, ... 단위로 지정한다.
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
