using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ColorText : MonoBehaviour
{
    public void OnEndEditValue()
    {
        string inputText = gameObject.GetComponent<TMP_InputField>().text;

        float enteredValue = inputText == "" ? 0 : float.Parse(inputText); // 만약 입력값이 없으면, 0으로 설정한다.

        if (enteredValue > 255)
        {
            enteredValue = 255;
        }
        else if (enteredValue < 0)
        {
            enteredValue = 0;
        }

        gameObject.GetComponent<TMP_InputField>().text = ((int) enteredValue).ToString(); // text에 변경 사항을 반영

        GetComponentInParent<Slider>().value = enteredValue; // 부모 slider에 변경 사항을 반영
    }
}
