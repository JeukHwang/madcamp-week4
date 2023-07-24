using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Vector2 openDirection = Vector2.zero;
    public bool shouldOpen = false;
    public float timeInSecond = 3.0f;

    Vector3 closedPosition;
    Vector3 openDirection3d;
    float current = 0f;
    float speed;

    // Start is called before the first frame update
    void Start()
    {
        speed = openDirection.magnitude / timeInSecond;
        closedPosition = transform.position;
        openDirection3d = new Vector3(openDirection.x, 0, openDirection.y);
    }

    // Update is called once per frame
    void Update()
    {
        float time = Time.deltaTime * (shouldOpen ? 1 : -1);
        current = Math.Max(Math.Min(current + time, timeInSecond), 0);
        transform.position = closedPosition + openDirection3d * speed * current;
    }
}
