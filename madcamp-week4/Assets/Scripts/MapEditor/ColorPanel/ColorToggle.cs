using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorToggle : MonoBehaviour
{
    Toggle rgbToggle;

    // Start is called before the first frame update
    void Start()
    {
        rgbToggle = gameObject.GetComponent<Toggle>();
        rgbToggle.onValueChanged.AddListener(delegate { OnToggleValueChanged(); }); // 토글에 리스너를 추가한다.
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnToggleValueChanged()
    {
        // 맵에서 오브젝트를 선택한 상태로 토글 색상을 바꾸는 중이다.
        // 따라서 변경한 토글 값을 오브젝트에 반영한다.
        if (MapEditor.userSelectedTarget != null)
        {
            switch (gameObject.name)
            {
                case "ToggleRed": // if는 torch를, else는 switch를 가리킨다.
                    if (MapEditor.userSelectedTarget.tag == "Torch") MapEditor.userSelectedTarget.GetComponent<TorchController>().red = rgbToggle.isOn;
                    else MapEditor.userSelectedTarget.GetComponent<TorchSwitchController>().red = rgbToggle.isOn;
                    break;

                case "ToggleGreen":
                    if (MapEditor.userSelectedTarget.tag == "Torch") MapEditor.userSelectedTarget.GetComponent<TorchController>().green = rgbToggle.isOn;
                    else MapEditor.userSelectedTarget.GetComponent<TorchSwitchController>().green = rgbToggle.isOn;
                    break;

                case "ToggleBlue":
                    if (MapEditor.userSelectedTarget.tag == "Torch") MapEditor.userSelectedTarget.GetComponent<TorchController>().blue = rgbToggle.isOn;
                    else MapEditor.userSelectedTarget.GetComponent<TorchSwitchController>().blue = rgbToggle.isOn;
                    break;
            }
        }
    }
}
