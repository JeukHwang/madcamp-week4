using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// item panel���� � �������� Ŭ���ϸ� ��� �۵��� �������� ��Ÿ����.
public class ItemButton : MonoBehaviour
{
    public static GameObject selectedItemButton = null;

    public GameObject ColorPanel;

    public void OnClickItemButton(GameObject newSelectedItemButton)
    {
        // �ش� ��ư�� ó�� ���� ���̶��, ������ �����Ѵ�.
        if (selectedItemButton != newSelectedItemButton && newSelectedItemButton != null)
        {
            // ������ ���� ������ �����Ѵ�.
            newSelectedItemButton.GetComponent<Image>().color = Color.yellow;

            // ������ ���õ� ���� �־��ٸ�, ������ ������� �����Ѵ�.
            if (selectedItemButton != null) selectedItemButton.GetComponent<Image>().color = Color.white;

            selectedItemButton = newSelectedItemButton;

            activateColorPanel();
        }
        // ���� ��ư�� �ٽ� �����ų�, ��ư�� �ƴ� �ٸ� ���� Ŭ���ߴ�.
        // ������ �����ߴ� ��ư�� �����ϰ�, color panel�� �����.
        else
        {
            selectedItemButton.GetComponent<Image>().color = Color.white;
            selectedItemButton = null;

            ColorPanel.transform.GetChild(1).gameObject.SetActive(false);
            ColorPanel.transform.GetChild(2).gameObject.SetActive(false);
        }
    }

    // ������ ������ ColorPanel�� Ȱ��ȭ�Ѵ�.
    private void activateColorPanel()
    {
        switch (selectedItemButton.name)
        {
            // Slider�� Ȱ��ȭ�ϰ�, Toggle�� ��Ȱ��ȭ�Ѵ�.
            case "ButtonPlayer":
            case "ButtonDoor":
            case "ButtonExit":
            case "ButtonWall":
                ColorPanel.transform.GetChild(1).gameObject.SetActive(true);
                ColorPanel.transform.GetChild(2).gameObject.SetActive(false);
                break;

            // Slider�� ��Ȱ��ȭ�ϰ�, Toggle�� Ȱ��ȭ�Ѵ�.
            case "ButtonSword":
            case "ButtonSwitch":
                ColorPanel.transform.GetChild(1).gameObject.SetActive(false);
                ColorPanel.transform.GetChild(2).gameObject.SetActive(true);
                break;
        }
    }
}
