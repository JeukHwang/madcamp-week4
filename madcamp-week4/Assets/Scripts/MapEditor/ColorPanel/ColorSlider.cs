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

        // 맵에서 오브젝트를 선택한 상태로 슬라이더 색상을 바꾸는 중이다.
        // 따라서 변경한 슬라이드 값을 오브젝트에 반영한다.
        if (MapEditor.userSelectedTarget != null)
        {
            switch (gameObject.name)
            {
                case "SliderRed":
                    changeTargetColor("red");
                    break;

                case "SliderGreen":
                    changeTargetColor("green");
                    break;

                case "SliderBlue":
                    changeTargetColor("blue");
                    break;

                case "SliderOpaque":
                    changeTargetColor("opaque");
                    break;
            }
        }
    }

    private void changeTargetColor(string colorType)
    {
        float red;
        float green;
        float blue;
        float opaque;
        Renderer targetRenderer;

        switch (MapEditor.userSelectedTarget.tag)
        {
            case "Player":
                targetRenderer = MapEditor.userSelectedTarget.transform.GetChild(1).GetComponent<Renderer>();
                red = targetRenderer.material.color.r;
                green = targetRenderer.material.color.g;
                blue = targetRenderer.material.color.b;
                opaque = targetRenderer.material.color.a;

                if (colorType == "red") targetRenderer.material.color = new Color(rgbSlider.value / 255f, green, blue, opaque);
                else if (colorType == "green") targetRenderer.material.color = new Color(red, rgbSlider.value / 255f, blue, opaque);
                else if (colorType == "blue") targetRenderer.material.color = new Color(red, green, rgbSlider.value / 255f, opaque);
                else targetRenderer.material.color = new Color(red, green, blue, rgbSlider.value / 255f);
                break;

            case "Door":
            case "Exit":
            case "Wall":
                targetRenderer = MapEditor.userSelectedTarget.GetComponent<Renderer>();
                red = targetRenderer.material.color.r;
                green = targetRenderer.material.color.g;
                blue = targetRenderer.material.color.b;
                opaque = targetRenderer.material.color.a;

                if (colorType == "red") targetRenderer.material.color = new Color(rgbSlider.value / 255f, green, blue, opaque);
                else if (colorType == "green") targetRenderer.material.color = new Color(red, rgbSlider.value / 255f, blue, opaque);
                else if (colorType == "blue") targetRenderer.material.color = new Color(red, green, rgbSlider.value / 255f, opaque);
                else targetRenderer.material.color = new Color(red, green, blue, rgbSlider.value / 255f);
                break;
        }
    }
}
