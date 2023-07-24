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

        float enteredValue = inputText == "" ? 0 : float.Parse(inputText); // ���� �Է°��� ������, 0���� �����Ѵ�.

        if (enteredValue > 255)
        {
            enteredValue = 255;
        }
        else if (enteredValue < 0)
        {
            enteredValue = 0;
        }

        gameObject.GetComponent<TMP_InputField>().text = ((int) enteredValue).ToString(); // text�� ���� ������ �ݿ�

        GetComponentInParent<Slider>().value = enteredValue; // �θ� slider�� ���� ������ �ݿ�
    }
}
