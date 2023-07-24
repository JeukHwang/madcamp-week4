using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlButton : MonoBehaviour
{   
    public void OnClickRotate(string direction)
    {
        if (MapEditor.userSelectedTarget != null)
        {
            switch (direction)
            {
                case "right":// clockwise
                    MapEditor.userSelectedTarget.transform.localEulerAngles += new Vector3(0, 90, 0);
                    break;
                case "left": // counterclockwise
                    MapEditor.userSelectedTarget.transform.localEulerAngles -= new Vector3(0, 90, 0);
                    Debug.Log("l");
                    break;
                default:
                    Debug.Log("h");
                    break;
            }
        }
    }
}
