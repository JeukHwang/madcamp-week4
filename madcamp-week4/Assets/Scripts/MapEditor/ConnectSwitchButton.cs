using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectSwitchButton : MonoBehaviour
{
    public void OnClickDoneButton()
    {
        // 안내 패널을 내리고, 비활성화한다.
        gameObject.SetActive(false);
        MapEditor.isSelectingSwitch = false;
    }
}
