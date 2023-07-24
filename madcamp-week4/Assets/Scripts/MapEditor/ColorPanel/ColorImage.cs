using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorImage : MonoBehaviour
{
    Image colorImage;

    public GameObject GameObjectR;
    public GameObject GameObjectG;
    public GameObject GameObjectB;
    public GameObject GameObjectA;

    // Start is called before the first frame update
    void Start()
    {
        colorImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        float r = GameObjectR.GetComponent<Slider>().value / 255;
        float g = GameObjectG.GetComponent<Slider>().value / 255;
        float b = GameObjectB.GetComponent<Slider>().value / 255;
        float a = GameObjectA.GetComponent<Slider>().value / 255;

        colorImage.color = new Color(r, g, b, a);
    }
}
