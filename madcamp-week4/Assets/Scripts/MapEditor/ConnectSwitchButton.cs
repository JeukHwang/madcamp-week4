using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectSwitchButton : MonoBehaviour
{
    public void OnClickDoneButton()
    {
        // �ȳ� �г��� ������, ��Ȱ��ȭ�Ѵ�.
        gameObject.SetActive(false);
        MapEditor.isSelectingSwitch = false;
    }
}
