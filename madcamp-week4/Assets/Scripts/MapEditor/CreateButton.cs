using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateButton : MonoBehaviour
{
    public static bool isPressed = false; // 현재 버튼이 눌러져 있는 상태인지를 가리킨다.
    public GameObject deleteButton;

    public void OnClickCreateButton()
    {
        isPressed = !isPressed;

        // 버튼 색을 바꾼다.
        if (isPressed) gameObject.GetComponent<Image>().color = new Color(1, 0.5f, 0); // 활성화 -> 주황색
        else gameObject.GetComponent<Image>().color = Color.white; // 비활성화 -> 하얀색

        // 생성 버튼 활성화 시 삭제 버튼을 비활성화한다.
        if (isPressed && DeleteButton.isPressed)
        {
            DeleteButton.isPressed = false;
            deleteButton.GetComponent<Image>().color = Color.white;
        }
    }
}
