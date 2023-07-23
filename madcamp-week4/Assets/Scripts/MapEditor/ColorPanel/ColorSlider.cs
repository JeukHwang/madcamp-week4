using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ColorSlider : MonoBehaviour
{
    Slider rgbSlider;
    TMP_InputField rgbText;

    // Start is called before the first frame update
    void Start()
    {
        rgbSlider = gameObject.GetComponent<Slider>();
        rgbSlider.onValueChanged.AddListener(delegate { OnSliderValueChanged(); }); // �����̴��� �����ʸ� �߰��Ѵ�.

        rgbText = transform.GetChild(0).GetComponent<TMP_InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSliderValueChanged()
    {
        // �����̴����� ������ [0, 255]�� ���� text�� �ݿ��Ѵ�.
        rgbText.text = ((int) rgbSlider.value).ToString();
    }
}
