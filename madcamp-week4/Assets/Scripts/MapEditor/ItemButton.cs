using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// item panel에서 어떤 아이템을 클릭하면 어떻게 작동할 것인지를 나타낸다.
public class ItemButton : MonoBehaviour
{
    public static GameObject selectedItemButton = null;

    public GameObject ColorPanel;

    public void OnClickItemButton(GameObject newSelectedItemButton)
    {
        // 해당 버튼을 처음 누른 것이라면, 선택을 적용한다.
        if (selectedItemButton != newSelectedItemButton && newSelectedItemButton != null)
        {
            // 선택한 것의 색상을 강조한다.
            newSelectedItemButton.GetComponent<Image>().color = Color.yellow;

            // 기존에 선택된 것이 있었다면, 색상을 원래대로 복구한다.
            if (selectedItemButton != null) selectedItemButton.GetComponent<Image>().color = Color.white;

            selectedItemButton = newSelectedItemButton;

            activateColorPanel();
        }
        // 같은 버튼을 다시 눌렀거나, 버튼이 아닌 다른 곳을 클릭했다.
        // 이전에 선택했던 버튼을 해제하고, color panel을 숨긴다.
        else
        {
            selectedItemButton.GetComponent<Image>().color = Color.white;
            selectedItemButton = null;

            ColorPanel.transform.GetChild(1).gameObject.SetActive(false);
            ColorPanel.transform.GetChild(2).gameObject.SetActive(false);
        }
    }

    // 선택한 것으로 ColorPanel을 활성화한다.
    private void activateColorPanel()
    {
        switch (selectedItemButton.name)
        {
            // Slider을 활성화하고, Toggle을 비활성화한다.
            case "ButtonPlayer":
            case "ButtonDoor":
            case "ButtonExit":
            case "ButtonWall":
                ColorPanel.transform.GetChild(1).gameObject.SetActive(true);
                ColorPanel.transform.GetChild(2).gameObject.SetActive(false);
                break;

            // Slider을 비활성화하고, Toggle을 활성화한다.
            case "ButtonSword":
            case "ButtonSwitch":
                ColorPanel.transform.GetChild(1).gameObject.SetActive(false);
                ColorPanel.transform.GetChild(2).gameObject.SetActive(true);
                break;
        }
    }
}
