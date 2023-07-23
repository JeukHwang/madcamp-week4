using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeleteButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickDeleteButton()
    {
        Destroy(MapEditor.userSelectedTarget);
        MapEditor.userSelectedTarget = null;

        gameObject.GetComponent<Button>().interactable = false;
    }
}
