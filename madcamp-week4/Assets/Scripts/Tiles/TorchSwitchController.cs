using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchSwitchController : MonoBehaviour
{
    public bool red;
    public bool green;
    public bool blue;
    public bool isActivated = false;

    public float detectDistance = 10.0f; // ???????? ?????????? ???? ???????? light?? ?????? ?????? ????

    Light switchLight;
    ParticleSystem.MainModule switchPS;
    GameObject sparks;
    GameObject fire;
    GameObject smoke;
    ParticleSystem.MainModule sparksPS;
    ParticleSystem.MainModule firePS;
    ParticleSystem.MainModule smokePS;

    float switchPSsize;
    bool isActivateInLastUpdate;

    // Start is called before the first frame update
    void Start()
    {
        switchLight = GetComponent<Light>();
        switchPS = GetComponent<ParticleSystem>().main;
        sparks = transform.GetChild(0).gameObject;
        fire = transform.GetChild(1).gameObject;
        smoke = transform.GetChild(2).gameObject;
        sparksPS = sparks.GetComponent<ParticleSystem>().main;
        firePS = fire.GetComponent<ParticleSystem>().main;
        smokePS = smoke.GetComponent<ParticleSystem>().main;

        applyProperty();
    }

    public void applyProperty()
    {
        Color targetColor = new Color(red ? 1.0f : 0.5f, green ? 1.0f : 0.5f, blue ? 1.0f : 0.5f, 0.5f);
        switchLight.color = targetColor;
        switchPS.startColor = targetColor;
        sparksPS.startColor = targetColor;
        firePS.startColor = targetColor;
        smokePS.startColor = targetColor;
    }

    // Update is called once per frame
    void Update()
    {
        // applyProperty();

        (float redDist, float greenDist, float blueDist) = closestDistanceToLight();
        bool isRedVisible = redDist <= detectDistance;
        bool isGreenVisible = greenDist <= detectDistance;
        bool isBlueVisible = blueDist <= detectDistance;

        Color currentColor = new Color(distanceToIntensity(redDist), distanceToIntensity(greenDist), distanceToIntensity(blueDist), 0.5f);
        switchLight.color = new Color(isRedVisible ? 1.0f : 0, isGreenVisible ? 1.0f : 0, isBlueVisible ? 1.0f : 0);
        sparksPS.startColor = new ParticleSystem.MinMaxGradient(currentColor);
        firePS.startColor = new ParticleSystem.MinMaxGradient(currentColor);
        smokePS.startColor = new ParticleSystem.MinMaxGradient(currentColor);

        isActivated = (isRedVisible == red) && (isGreenVisible == green) && (isBlueVisible == blue);
        float targetSize = isActivated ? 2.2f : 0.5f;
        switchPSsize = Mathf.Lerp(switchPSsize, targetSize, 0.1f);
        switchPS.startSize = new ParticleSystem.MinMaxCurve(switchPSsize);
        if (isActivated != isActivateInLastUpdate)
        {
            sparks.SetActive(isActivated);
            switchPS.startSize = 20f;
        }
        isActivateInLastUpdate = isActivated;
    }

    private (float, float, float) closestDistanceToLight()
    {
        float red = float.PositiveInfinity;
        float green = float.PositiveInfinity;
        float blue = float.PositiveInfinity;
        foreach (GameObject torch in GameObject.FindGameObjectsWithTag("Torch"))
        {
            Vector3 rayDirection = (torch.transform.position - transform.position).normalized;
            RaycastHit rayHit;
            bool isHitted = Physics.Raycast(transform.position, rayDirection, out rayHit, detectDistance * 2);
            Debug.DrawRay(transform.position, rayDirection * detectDistance, Color.cyan, 0.1f);
            Debug.DrawRay(transform.position + new Vector3(0.01f, 0.01f, 0.01f), rayDirection * detectDistance * 2 + new Vector3(0.01f, 0.01f, 0.01f), Color.magenta, 0.1f);
            if (isHitted)
            {
                Debug.DrawLine(transform.position - new Vector3(0.01f, 0.01f, 0.01f), rayHit.point - new Vector3(0.01f, 0.01f, 0.01f), Color.green, 0.1f);
                bool isNotBlockedByOtherGameObject = rayHit.collider.gameObject == torch;
                if (isNotBlockedByOtherGameObject)
                {
                    TorchController torchController = torch.GetComponent<TorchController>();
                    float distance = Vector3.Distance(torch.transform.position, transform.position);
                    if (torchController.red)
                    {
                        red = Math.Min(distance, red);
                    }
                    if (torchController.green)
                    {
                        green = Math.Min(distance, green);
                    }
                    if (torchController.blue)
                    {
                        blue = Math.Min(distance, blue);
                    }
                }
            }
        }
        return (red, green, blue);
    }

    private float distanceToIntensity(float distance)
    {
        float relativeDistance = distance / detectDistance;
        float intensity;
        if (relativeDistance <= 1)
        {
            intensity = 1.0f;
        }
        else if (relativeDistance <= 2)
        {
            intensity = 2 - relativeDistance;
        }
        else
        {
            intensity = 0.0f;
        }
        return (intensity / 2) + 0.5f;
    }
}