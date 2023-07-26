using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public bool isTransparent = false;
    public List<TorchSwitchController> switchControllers = new List<TorchSwitchController>();
    public bool shouldOpen = false;
    public Material opaqueMaterial;
    public Material transparentMaterial;

    Vector2 openDirection = new Vector2(2, 0);
    float timeInSecond = 3.0f;

    public Vector3 closedPosition;
    Vector3 openDirection3d;
    float current = 0f;
    float speed;

    int LayerDefault;
    int LayerIgnoreRaycast;
    Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
        speed = openDirection.magnitude / timeInSecond;
        closedPosition = transform.position;
        openDirection3d = new Vector3(openDirection.x, 0, openDirection.y);

        rend = GetComponent<Renderer>();
        LayerDefault = LayerMask.NameToLayer("Default");
        LayerIgnoreRaycast = LayerMask.NameToLayer("Ignore Raycast");

        applyProperty();
    }

    public void applyProperty()
    {
        Color prevColor = rend.material.color;

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
        rend.material.color = prevColor;
    }

    // Update is called once per frame
    void Update()
    {
        // applyProperty();

        if (switchControllers.ToArray().Length == 0)
        {
            shouldOpen = false;
        }
        else
        {
            shouldOpen = switchControllers.TrueForAll(switchController => switchController.isActivated);
        }

        float time = Time.deltaTime * (shouldOpen ? 1 : -1);
        current = Math.Max(Math.Min(current + time, timeInSecond), 0);
        transform.position = closedPosition + transform.TransformDirection(openDirection3d).normalized * speed * current;
    }
}
