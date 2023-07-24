using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// item panel���� � �������� Ŭ���ϸ� ��� �۵��� �������� ��Ÿ����.
public class ItemPanel : MonoBehaviour
{
    public static GameObject selectedItem = null;

    public GameObject createButton;

    public GameObject ColorPanel;

    public void OnClickItemButton(GameObject newSelectedItem)
    {
        // �ش� ��ư�� ó�� ���� ���̶��, ������ �����Ѵ�.
        if (selectedItem != newSelectedItem && newSelectedItem != null)
        {
            // ������ ���� ������ �����Ѵ�.
            newSelectedItem.GetComponent<Image>().color = new Color(1, 1, 0.8f); // ���� ���

            // ������ ���õ� ���� �־��ٸ�, ������ ������� �����Ѵ�.
            if (selectedItem != null) selectedItem.GetComponent<Image>().color = Color.white;
            // �����ٸ�, create ��ư�� Ȱ��ȭ�Ѵ�.
            else createButton.GetComponent<Button>().interactable = true;

            selectedItem = newSelectedItem;

            activateColorPanel();
        }
        // ���� ��ư�� �ٽ� �����ų�, ��ư�� �ƴ� �ٸ� ���� Ŭ���ߴ�.
        // ������ �����ϰ� create ��ư�� ��Ȱ��ȭ�Ѵ�.
        else
        {
            selectedItem.GetComponent<Image>().color = Color.white;
            selectedItem = null;

            createButton.GetComponent<Button>().interactable = false;

            ColorPanel.transform.GetChild(1).gameObject.SetActive(false);
            ColorPanel.transform.GetChild(2).gameObject.SetActive(false);
        }
    }

    // ������ ������ ColorPanel�� Ȱ��ȭ�Ѵ�.
    private void activateColorPanel()
    {
        switch (selectedItem.name)
        {
            // Slider�� Ȱ��ȭ�ϰ�, Toggle�� ��Ȱ��ȭ�Ѵ�.
            case "ButtonExit":
            case "ButtonWall":
                ColorPanel.transform.GetChild(1).gameObject.SetActive(true);
                ColorPanel.transform.GetChild(2).gameObject.SetActive(false);
                break;

            // Slider�� ��Ȱ��ȭ�ϰ�, Toggle�� Ȱ��ȭ�Ѵ�.
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
