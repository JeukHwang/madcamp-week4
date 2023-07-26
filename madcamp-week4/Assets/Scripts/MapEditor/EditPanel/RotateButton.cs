using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using UnityEngine.InputSystem;

public class RotateButton : MonoBehaviour
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
                    break;

                default:
                    break;
            }
        }
    }
}
