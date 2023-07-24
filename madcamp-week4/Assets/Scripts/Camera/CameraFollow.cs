using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float smoothSpeed = 2f;
    public Vector3 positionOffset;
    public Vector3 lookOffset;

    Transform target;

    private void Start()
    {
        target = transform.parent;
    }

    void LateUpdate()
    {
        Vector3 desiredPosition = transform.parent.position + target.TransformDirection(positionOffset);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
        transform.LookAt(target.position + target.TransformDirection(lookOffset));
    }
}
