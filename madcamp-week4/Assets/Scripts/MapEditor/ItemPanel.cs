using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// item panel에서 어떤 아이템을 클릭하면 어떻게 작동할 것인지를 나타낸다.
public class ItemPanel : MonoBehaviour
{
    public static GameObject selectedItem = null;

    public GameObject createButton;

    public GameObject ColorPanel;

    public void OnClickItemButton(GameObject newSelectedItem)
    {
        // 해당 버튼을 처음 누른 것이라면, 선택을 적용한다.
        if (selectedItem != newSelectedItem && newSelectedItem != null)
        {
            // 선택한 것의 색상을 강조한다.
            newSelectedItem.GetComponent<Image>().color = new Color(1, 1, 0.8f); // 연한 노랑

            // 기존에 선택된 것이 있었다면, 색상을 원래대로 복구한다.
            if (selectedItem != null) selectedItem.GetComponent<Image>().color = Color.white;
            // 없었다면, create 버튼을 활성화한다.
            else createButton.GetComponent<Button>().interactable = true;

            selectedItem = newSelectedItem;

            activateColorPanel();
        }
        // 같은 버튼을 다시 눌렀거나, 버튼이 아닌 다른 곳을 클릭했다.
        // 선택을 해제하고 create 버튼을 비활성화한다.
        else
        {
            selectedItem.GetComponent<Image>().color = Color.white;
            selectedItem = null;

            createButton.GetComponent<Button>().interactable = false;

            ColorPanel.transform.GetChild(1).gameObject.SetActive(false);
            ColorPanel.transform.GetChild(2).gameObject.SetActive(false);
        }
    }

    // 선택한 것으로 ColorPanel을 활성화한다.
    private void activateColorPanel()
    {
        switch (selectedItem.name)
        {
            // Slider을 활성화하고, Toggle을 비활성화한다.
            case "ButtonExit":
            case "ButtonWall":
                ColorPanel.transform.GetChild(1).gameObject.SetActive(true);
                ColorPanel.transform.GetChild(2).gameObject.SetActive(false);
                break;

            // Slider을 비활성화하고, Toggle을 활성화한다.
            case "ButtonPlayer":
            case "ButtonLight":
            case "ButtonSwitch":
            case "ButtonDoor":
                ColorPanel.transform.GetChild(1).gameObject.SetActive(false);
                ColorPanel.transform.GetChild(2).gameObject.SetActive(true);
                break;
        }
    }
}
