using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchController : MonoBehaviour
{
    public bool red;
    public bool green;
    public bool blue;
    public bool on = true;

    Light innerLight;

    // Start is called before the first frame update
    void Start()
    {
        innerLight = gameObject.GetComponentInChildren<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        innerLight.gameObject.SetActive(on);
        innerLight.color = new Color(red ? 1 : 0, green ? 1 : 0, blue ? 1 : 0 );
    }
}
