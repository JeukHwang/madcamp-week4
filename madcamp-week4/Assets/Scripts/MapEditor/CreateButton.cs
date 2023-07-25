using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateButton : MonoBehaviour
{
    public static bool isPressed = false; // ���� ��ư�� ������ �ִ� ���������� ����Ų��.
    public GameObject deleteButton;

    public void OnClickCreateButton()
    {
        isPressed = !isPressed;

        // ��ư ���� �ٲ۴�.
        if (isPressed) gameObject.GetComponent<Image>().color = new Color(1, 0.5f, 0); // Ȱ��ȭ -> ��Ȳ��
        else gameObject.GetComponent<Image>().color = Color.white; // ��Ȱ��ȭ -> �Ͼ��

        // ���� ��ư Ȱ��ȭ �� ���� ��ư�� ��Ȱ��ȭ�Ѵ�.
        if (isPressed && DeleteButton.isPressed)
        {
            DeleteButton.isPressed = false;
            deleteButton.GetComponent<Image>().color = Color.white;
        }
    }
}
