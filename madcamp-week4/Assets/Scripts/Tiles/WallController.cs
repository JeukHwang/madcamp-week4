using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour
{
    public bool isTransparent = false;
    public Material opaqueMaterial;
    public Material transparentMaterial;

    int LayerDefault;
    int LayerIgnoreRaycast;
    Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        LayerDefault = LayerMask.NameToLayer("Default");
        LayerIgnoreRaycast = LayerMask.NameToLayer("Ignore Raycast");
        updateProperty();
    }

    void updateProperty()
    {
        if (isTransparent)
        {
            rend.sharedMaterial = transparentMaterial;
            gameObject.layer = LayerIgnoreRaycast;
        }
        else
        {
            rend.sharedMaterial = opaqueMaterial;
            gameObject.layer = LayerDefault;
        }
    }

    // Update is called once per frame
    void Update()
    {
        updateProperty();
    }
}
