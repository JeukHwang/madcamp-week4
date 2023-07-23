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
        rgbSlider.onValueChanged.AddListener(delegate { OnSliderValueChanged(); }); // 슬라이더에 리스너를 추가한다.

        rgbText = transform.GetChild(0).GetComponent<TMP_InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSliderValueChanged()
    {
        // 슬라이더에서 조절한 [0, 255]의 값을 text에 반영한다.
        rgbText.text = ((int) rgbSlider.value).ToString();
    }
}
