using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchController : MonoBehaviour
{
    public bool red = true;
    public bool green = true;
    public bool blue = true;
    public bool on = true;

    // Start is called before the first frame update
    void Start()
    {
        applyProperty();
    }

    public void applyProperty()
    {
        Light innerLight = gameObject.GetComponentInChildren<Light>();
        innerLight.enabled = on;
        innerLight.color = new Color(red ? 1 : 0, green ? 1 : 0, blue ? 1 : 0);
    }

    // Update is called once per frame
    void Update()
    {
        // applyProperty();
    }
}
