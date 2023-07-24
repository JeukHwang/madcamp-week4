using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// item panel���� � �������� Ŭ���ϸ� ��� �۵��� �������� ��Ÿ����.
public class ItemPanel : MonoBehaviour
{
    public static GameObject selectedItem = null;

    public GameObject createButton;

    public void OnClickItemButton(GameObject newSelectedItem)
    {
        // ó�� ���� ���̶��, ������ �����Ѵ�.
        if (selectedItem != newSelectedItem)
        {
            // ������ ���� ������ �����Ѵ�.
            newSelectedItem.GetComponent<Image>().color = new Color(1, 1, 0.8f); // ���� ���

            // ������ ���õ� ���� �־��ٸ�, ������ ������� �����Ѵ�.
            if (selectedItem != null) selectedItem.GetComponent<Image>().color = Color.white;
            // �����ٸ�, create ��ư�� Ȱ��ȭ�Ѵ�.
            else createButton.GetComponent<Button>().interactable = true;

            selectedItem = newSelectedItem;
        }
        else // �ٽ� ���� ���̶��, ������ �����ϰ� create ��ư�� ��Ȱ��ȭ�Ѵ�.
        {
            selectedItem.GetComponent<Image>().color = Color.white;
            selectedItem = null;

            createButton.GetComponent<Button>().interactable = false;
        }
    }
}
