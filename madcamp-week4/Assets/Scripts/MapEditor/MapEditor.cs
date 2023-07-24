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
    GameObject userSelectedItemButton = null;

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
    public GameObject Factory6;
    public GameObject Factory7;

    // 색깔 조정하는 패널
    public GameObject ColorPanel;

    public GameObject itemPanel;


    // Start is called before the first frame update
    void Start()
    {

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

                // 만약 item panel에서 클릭했던 것이 있었으면, 해제한다.
                if (ItemPanel.selectedItem != null) itemPanel.GetComponent<ItemPanel>().OnClickItemButton(null);

                // 마우스 클릭한 화면 좌표를 레이캐스트로 변경하여 3D 월드 좌표로 얻기
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit)) // 레이캐스트가 아무 물체에 부딪힌 경우
                {
                    Debug.Log("부딪힌 3D 월드 좌표: " + hit.point);

                    // 만약 클릭한 대상이 바닥인 경우에는 (선택 대상을 초기화한다.
                    if (hit.collider.gameObject.CompareTag("Plane"))
                    {
                        userSelectedTarget = null;
                        deleteButton.GetComponent<Button>().interactable = false;

                        ColorPanel.transform.GetChild(1).gameObject.SetActive(false);
                        ColorPanel.transform.GetChild(2).gameObject.SetActive(false);
                    }
                    else // 그렇지 않은 경우 대상을 선택한다. (수정, 이동, 삭제 등등 활성화)
                    { 
                        userSelectedTarget = hit.collider.gameObject;
                        userKeepedTarget = hit.collider.gameObject;
                        deleteButton.GetComponent<Button>().interactable = true;
                    }
                }
            }
            // UI 요소를 선택했다. (아이템을 드래그 앤 드랍으로 instantiate할 수 있게 한다)
            else
            {
                // item panel 위에서 클릭한 버튼을 가리킨다.
                userSelectedItemButton = EventSystem.current.currentSelectedGameObject;
                Debug.Log(userSelectedItemButton.name);
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
                    // 움직이는 마우스 위치로 대상을 이동시킨다.
                    userKeepedTarget.transform.position = normalizePosition(hit.point.x, 0, hit.point.z);
                }
            }
            // item panel 위에서 아이템을 클릭한 다음에 마우스로 계속 누르고 있다.
            else if (userSelectedItemButton != null)
            {
                // 마우스를 꾹 누른 상태로 마우스 위치를 맵 위로 최초로 이동했다.
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
                        userCreateObject.transform.position = normalizePosition(hit.point.x, 0, hit.point.z);
                    }
                }
            }            
        }

        if (Input.GetMouseButtonUp(0))
        {
            userKeepedTarget = null; // 꾹 누르고 있었던, 기존에 맵에 있던 오브젝트
            userSelectedItemButton = null; // 꾹 누르고 있었던, item panel 위의 버튼

            // 현재 item panel에서 드래그해서 오브젝트를 맵 상에서 생성하려고 한다.
            if (userCreateObject != null)
            {
                // 현재 마우스 위치가 맵 위에 있지 않고, UI 위에 있다.
                if (EventSystem.current.IsPointerOverGameObject()) { Destroy(userCreateObject); } // 생성하지 않고 삭제한다.
                userSelectedItemButton = null;
                userCreateObject = null;
            }
            // 사용자가 맵 상에서 오브젝트를 선택했다.
            else if (userSelectedTarget != null && !EventSystem.current.IsPointerOverGameObject())
            {
                activateColorPanel();
            }
            
        }
    }

    // Color panel을 활성화하고, 선택한 오브젝트의 색깔을 불러온다.
    private void activateColorPanel()
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

    // 위치를 0, 2, 4, ... 단위로 지정한다.
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
