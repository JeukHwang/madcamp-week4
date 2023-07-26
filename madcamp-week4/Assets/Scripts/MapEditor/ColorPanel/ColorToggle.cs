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
        rgbToggle.onValueChanged.AddListener(delegate { OnToggleValueChanged(); }); // ��ۿ� �����ʸ� �߰��Ѵ�.
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnToggleValueChanged()
    {
        // �ʿ��� ������Ʈ�� ������ ���·� ��� ������ �ٲٴ� ���̴�.
        // ���� ������ ��� ���� ������Ʈ�� �ݿ��Ѵ�.
        if (MapEditor.userSelectedTarget != null)
        {
            switch (gameObject.name)
            {
                case "ToggleRed": // if�� torch��, else�� switch�� ����Ų��.
                    if (MapEditor.userSelectedTarget.tag == "Torch")
                    {
                        // Debug.Log(MapEditor.userSelectedTarget.GetComponent<TorchController>());
                        MapEditor.userSelectedTarget.GetComponent<TorchController>().red = rgbToggle.isOn;
                        MapEditor.userSelectedTarget.GetComponent<TorchController>().applyProperty();
                    }
                    else
                    {
                        MapEditor.userSelectedTarget.GetComponent<TorchSwitchController>().red = rgbToggle.isOn;
                        MapEditor.userSelectedTarget.GetComponent<TorchSwitchController>().applyProperty();
                    }
                    break;

                case "ToggleGreen":
                    if (MapEditor.userSelectedTarget.tag == "Torch")
                    {
                        MapEditor.userSelectedTarget.GetComponent<TorchController>().green = rgbToggle.isOn;
                        MapEditor.userSelectedTarget.GetComponent<TorchController>().applyProperty();
                    }
                    else
                    {
                        MapEditor.userSelectedTarget.GetComponent<TorchSwitchController>().green = rgbToggle.isOn;
                        MapEditor.userSelectedTarget.GetComponent<TorchSwitchController>().applyProperty();
                    }
                    break;

                case "ToggleBlue":
                    if (MapEditor.userSelectedTarget.tag == "Torch")
                    {
                        MapEditor.userSelectedTarget.GetComponent<TorchController>().blue = rgbToggle.isOn;
                        MapEditor.userSelectedTarget.GetComponent<TorchController>().applyProperty();
                    }
                    else
                    {
                        MapEditor.userSelectedTarget.GetComponent<TorchSwitchController>().blue = rgbToggle.isOn;
                        MapEditor.userSelectedTarget.GetComponent<TorchSwitchController>().applyProperty();
                    }
                    break;
            }
        }
    }
}
