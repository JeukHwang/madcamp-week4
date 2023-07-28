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

    public void TransparentToggleChanged(GameObject toggle)
    {
        if (MapEditor.userSelectedTarget != null)
        {
            switch (MapEditor.userSelectedTarget.tag)
            {
                case "Door":
                    MapEditor.userSelectedTarget.GetComponent<DoorController>().isTransparent = toggle.GetComponent<Toggle>().isOn;
                    MapEditor.userSelectedTarget.GetComponent<DoorController>().applyProperty();
                    break;

                case "Wall":
                    MapEditor.userSelectedTarget.GetComponent<WallController>().isTransparent = toggle.GetComponent<Toggle>().isOn;
                    MapEditor.userSelectedTarget.GetComponent<WallController>().applyProperty();
                    break;
            }
        }
    }

    public void OnSliderValueChanged()
    {
        // �����̴����� ������ [0, 255]�� ���� text�� �ݿ��Ѵ�.
        rgbText.text = ((int) rgbSlider.value).ToString();

        // �ʿ��� ������Ʈ�� ������ ���·� �����̴� ������ �ٲٴ� ���̴�.
        // ���� ������ �����̵� ���� ������Ʈ�� �ݿ��Ѵ�.
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
                targetRenderer = MapEditor.userSelectedTarget.transform.GetChild(2).GetComponent<Renderer>();
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
            // case "Exit":
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
