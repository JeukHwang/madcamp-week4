using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateButton : MonoBehaviour
{
    public GameObject PlayerFactory;
    public GameObject LightFactory;
    public GameObject SwitchFactory;
    public GameObject DoorFactory;
    public GameObject ExitFactory;
    public GameObject WallFactory;
    public GameObject Factory6;
    public GameObject Factory7;

    public GameObject ColorImage; // 사용자가 설정한 색상


    public void OnClickCreateButton()
    {
        GameObject createdObject = null;
        Debug.Log(ItemPanel.selectedItem);

        Color newColor = ColorImage.GetComponent<Image>().color;

        switch (ItemPanel.selectedItem.transform.name)
        {
            case "ButtonPlayer":
                createdObject = Instantiate(PlayerFactory, new Vector3(0, 1, 0), Quaternion.identity);
                createdObject.transform.GetChild(1).GetComponent<Renderer>().material.color = newColor; // Alpha_Surface 색상 변경
                break;

            case "ButtonLight":
                createdObject = Instantiate(LightFactory, new Vector3(0, 1, 0), Quaternion.identity);
                TorchController lightScript = createdObject.GetComponent<TorchController>();

                if (newColor.r == 1) lightScript.red = true;
                else lightScript.red = false;

                if (newColor.g == 1) lightScript.green = true;
                else lightScript.green = false;

                if (newColor.b == 1) lightScript.blue = true;
                else lightScript.blue = false;
                break;

            case "ButtonSwitch":
                createdObject = Instantiate(SwitchFactory, new Vector3(0, 1, 0), Quaternion.identity);
                TorchSwitchController switchScript = createdObject.GetComponent<TorchSwitchController>();

                if (newColor.r == 1) switchScript.red = true;
                else switchScript.red = false;

                if (newColor.g == 1) switchScript.green = true;
                else switchScript.green = false;

                if (newColor.b == 1) switchScript.blue = true;
                else switchScript.blue = false;
                break;

            case "ButtonDoor":
                createdObject = Instantiate(DoorFactory, new Vector3(0, 1, 0), Quaternion.identity);
                createdObject.GetComponent<Renderer>().material.color = newColor;
                break;

            case "ButtonExit":
                createdObject = Instantiate(ExitFactory, new Vector3(0, 1, 0), Quaternion.identity);
                createdObject.GetComponent<Renderer>().material.color = newColor;
                break;

            case "ButtonWall":
                createdObject = Instantiate(WallFactory, new Vector3(0, 1, 0), Quaternion.identity);
                createdObject.GetComponent<Renderer>().material.color = newColor;
                break;
        }

       
    }
}
