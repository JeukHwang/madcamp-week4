using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorImage : MonoBehaviour
{
    Image colorImage;


    public GameObject Slider;

    Slider SliderRed;
    Slider SliderGreen;
    Slider SliderBlue;
    Slider SliderOpaque;

    public GameObject Toggle;

    Toggle ToggleRed;
    Toggle ToggleGreen;
    Toggle ToggleBlue;


    // Start is called before the first frame update
    void Start()
    {
        colorImage = GetComponent<Image>();

        SliderRed = Slider.transform.GetChild(0).GetComponent<Slider>();
        SliderGreen = Slider.transform.GetChild(1).GetComponent<Slider>();
        SliderBlue = Slider.transform.GetChild(2).GetComponent<Slider>();
        SliderOpaque = Slider.transform.GetChild(3).GetComponent<Slider>();

        ToggleRed = Toggle.transform.GetChild(0).GetComponent<Toggle>();
        ToggleGreen = Toggle.transform.GetChild(1).GetComponent<Toggle>();
        ToggleBlue = Toggle.transform.GetChild(2).GetComponent<Toggle>();
    }

    // Update is called once per frame
    void Update()
    {
        float r;
        float g;
        float b;
        float a;

        if (Slider.activeSelf)
        {
            r = SliderRed.value / 255f;
            g = SliderGreen.value / 255f;
            b = SliderBlue.value / 255f;
            a = SliderOpaque.value / 255f;
        }
        else
        {
            r = ToggleRed.isOn ? 1 : 0;
            g = ToggleGreen.isOn ? 1 : 0;
            b = ToggleBlue.isOn ? 1 : 0;
            a = 1;
        }
        
        colorImage.color = new Color(r, g, b, a);
    }
}
