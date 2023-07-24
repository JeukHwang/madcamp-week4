using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateButton : MonoBehaviour
{
    public GameObject PlayerFactory;
    public GameObject LightFactory;
    public GameObject SwitchFactory;
    public GameObject DoorFactory;
    public GameObject ExitFactory;
    public GameObject WallFactory;
    public GameObject Factory6;
    public GameObject Factory7;

    public GameObject ColorImage; // 사용자가 설정한 색상

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClickCreateButton()
    {
        GameObject instantiateTarget = null;

        Debug.Log(ItemPanel.selectedItem);

        switch (ItemPanel.selectedItem.transform.name)
        {
            case "ButtonPlayer":
                break;

            case "ButtonLight":
                break;

            case "ButtonSwitch":
                Debug.Log("true");
                instantiateTarget = SwitchFactory;
                break;
        }

        GameObject obj = Instantiate(instantiateTarget, new Vector3(0, 1, 0), Quaternion.identity);
    }
}
