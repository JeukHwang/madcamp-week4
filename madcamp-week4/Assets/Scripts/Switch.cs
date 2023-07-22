using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;

public class Switch : MonoBehaviour
{
    public GameObject targetLight1; // �� ����ġ�� �۵��ϱ� ���� �ʿ��� light�̴�.
    public GameObject targetLight2 = null;
    public GameObject targetLight3 = null;
    int lightCount; // �ʿ��� light ����

    public GameObject targetDoor; // �� ����ġ�� ����Ǿ� �ִ� ���̴�.
    public float detectDistance = 2f; // ����ġ�� Ȱ��ȭ�Ǳ� ���� �䱸�Ǵ� light�� ����ġ ������ �Ÿ�

    bool isClose; // ������ �Ÿ��� ����� ������� ����
    bool isActive; // ���� ����ġ Ȱ��ȭ ����

    // ray�� �¾��� ���� ������ ��´�.
    RaycastHit rayHit1; 
    RaycastHit rayHit2; 
    RaycastHit rayHit3;

    // ray�� light�� �ٶ󺸴� ����
    Vector3 rayDirection1;
    Vector3 rayDirection2;
    Vector3 rayDirection3;

    ParticleSystem.MinMaxGradient originFireColor; // ������ Fire�� ����

    ParticleSystem.MainModule switchPS; // �ش� ����ġ�� ParticleSystem�� MainModule
    ParticleSystem.MainModule firePS; // �ش� ����ġ�� �ڽ��� Fire�� ParticleSystem�� MainModule

    // Start is called before the first frame update
    void Start()
    {
        isActive = false;

        switchPS = GetComponent<ParticleSystem>().main;
        firePS = transform.GetChild(1).GetComponent<ParticleSystem>().main;

        originFireColor = firePS.startColor;

        if (targetLight2 == null) lightCount = 1;
        else if (targetLight3 == null) lightCount = 2;
        else lightCount = 3;
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.DrawRay(transform.position, rayDirection, Color.red, 0.4f);

        // ray�� ���� ���� ���� ù ��° ������Ʈ�� targetLight�̴�.
        // ���� ray�� light ���̿� ��ֹ��� ������ if���� false�� �Ǿ� ����ġ�� Ȱ��ȭ���� ���� ���̴�.
        switch (lightCount)
        {
            case 1:
                lightCount1();
                break;

            case 2:
                lightCount2();
                break;

            case 3:
                lightCount3();
                break;
        }
        

        if (isActive)  // ����� ������ ������ �����Ѵ�.
        {
            targetDoor.GetComponent<Door>().shouldOpen = true;

            transform.GetChild(0).gameObject.SetActive(true); // Sparks Ȱ��ȭ
            transform.GetChild(1).gameObject.SetActive(true); // Fire Ȱ��ȭ
            transform.GetChild(2).gameObject.SetActive(true); // Smoke Ȱ��ȭ
        }
        else if (isClose) // ������ ���������� ������, ����� ������.
        {
            targetDoor.GetComponent<Door>().shouldOpen= false;

            transform.GetChild(0).gameObject.SetActive(false); // Sparks ��Ȱ��ȭ
            transform.GetChild(1).gameObject.SetActive(true); // Fire Ȱ��ȭ (������ �� ����� ����� fire�� Ȱ��ȭ�ȴ�)
            transform.GetChild(2).gameObject.SetActive(false); // Smoke ��Ȱ��ȭ
        }
        else
        {
            targetDoor.GetComponent<Door>().shouldOpen = false;

            transform.GetChild(0).gameObject.SetActive(false); // Sparks ��Ȱ��ȭ
            transform.GetChild(1).gameObject.SetActive(false); // Fire ��Ȱ��ȭ
            transform.GetChild(2).gameObject.SetActive(false); // Smoke ��Ȱ��ȭ
        }
    }

    private void lightCount1() // Ÿ�� light�� 1���̴�.
    {
        rayDirection1 = (targetLight1.transform.position - transform.position).normalized;

        // Ÿ���� ���� �� ray�� �ε�����, ����� Ÿ���̴�.
        if (Physics.Raycast(transform.position, rayDirection1, out rayHit1, detectDistance + 5) && rayHit1.collider.gameObject == targetLight1)
        {
            if (rayHit1.distance <= detectDistance) // �Ÿ��� ����� ����� ����ġ�� Ȱ��ȭ�ȴ�.
            {
                isClose = true;
                isActive = true;
                switchPS.startSize = 2.2f;

            }
            else // �Ÿ��� �ణ ����� �Һ� ũ�Ⱑ ���� Ŀ����.
            {
                slightlyClose(rayHit1.distance);
            }
        }
        else // ��~�� �ְų�, �ε��� ����� Ÿ���� �ƴϴ�.
        {
            cannotActivate();
        }
    }

    private void lightCount2() // Ÿ�� light�� 2���̴�.
    {
        rayDirection1 = (targetLight1.transform.position - transform.position).normalized;
        rayDirection2 = (targetLight2.transform.position - transform.position).normalized;

        // ���� ������ �浹 ���� Ȯ��
        bool ray1 = Physics.Raycast(transform.position, rayDirection1, out rayHit1, detectDistance + 5);
        bool ray2 = Physics.Raycast(transform.position, rayDirection2, out rayHit2, detectDistance + 5);

        // �� �� ���� ������ �浹�� �ְ�, �浹 ��� ��� Ÿ���̴�.
        if (ray1 && ray2 && rayHit1.collider.gameObject == targetLight1 && rayHit2.collider.gameObject == targetLight2) 
        {
            if (rayHit1.distance <= detectDistance && rayHit2.distance <= detectDistance) // �� �� �Ÿ��� ����� ����� ����ġ�� Ȱ��ȭ�ȴ�.
            {
                canActivate();

            }
            else if (rayHit1.distance <= detectDistance) 
            {
                veryClose(rayHit1.collider.GetComponent<Light>().color); // light1�� �������� Ÿ������.
            }
            else if (rayHit2.distance <= detectDistance) 
            {
                veryClose(rayHit2.collider.GetComponent<Light>().color); // light2�� �������� Ÿ������.
            }
            else // �Ÿ��� �ణ ����� �Һ� ũ�Ⱑ ���� Ŀ����.
            {
                slightlyClose(Mathf.Min(rayHit1.distance, rayHit2.distance));
            }
        }
        else if (ray1 && rayHit1.collider.gameObject == targetLight1) // Ÿ�� 1�� ���� ���̴�.
        {
            if (rayHit1.distance <= detectDistance) 
            {
                veryClose(rayHit1.collider.GetComponent<Light>().color); // light1�� �������� Ÿ������.
            }
            else 
            {
                slightlyClose(rayHit1.distance);
            }
        }
        else if (ray2 && rayHit2.collider.gameObject == targetLight2) // Ÿ�� 2�� ���� ���̴�.
        {
            if (rayHit2.distance <= detectDistance) 
            {
                veryClose(rayHit2.collider.GetComponent<Light>().color); // light2�� �������� Ÿ������.
            }
            else
            {
                slightlyClose(rayHit2.distance);
            }
        }
        else // �� �� ��~�� �ִ�.
        {
            cannotActivate();
        }
    }

    private void lightCount3() // Ÿ�� light�� 3���̴�.
    {
        rayDirection1 = (targetLight1.transform.position - transform.position).normalized;
        rayDirection2 = (targetLight2.transform.position - transform.position).normalized;
        rayDirection3 = (targetLight3.transform.position - transform.position).normalized;

        // ���� ������ �浹 ���� Ȯ��
        bool ray1 = Physics.Raycast(transform.position, rayDirection1, out rayHit1, detectDistance + 5);
        bool ray2 = Physics.Raycast(transform.position, rayDirection2, out rayHit2, detectDistance + 5);
        bool ray3 = Physics.Raycast(transform.position, rayDirection3, out rayHit3, detectDistance + 5);

        // �� �� ���� ������ �浹�� �ְ�, �浹 ��� ��� Ÿ���̴�.
        if (ray1 && ray2 && ray3 && rayHit1.collider.gameObject == targetLight1 && rayHit2.collider.gameObject == targetLight2 && rayHit3.collider.gameObject == targetLight3)
        {
            if (rayHit1.distance <= detectDistance && rayHit2.distance <= detectDistance && rayHit3.distance <= detectDistance) // �� �� �Ÿ��� ����� ����� ����ġ�� Ȱ��ȭ�ȴ�.
            {
                canActivate();

            }
            else if (rayHit1.distance <= detectDistance && rayHit2.distance <= detectDistance)
            {
                veryClose(rayHit1.collider.GetComponent<Light>().color, rayHit2.collider.GetComponent<Light>().color); // light1�� 2�� �������� Ÿ������.
            }
            else if (rayHit1.distance <= detectDistance && rayHit3.distance <= detectDistance)
            {
                veryClose(rayHit1.collider.GetComponent<Light>().color, rayHit3.collider.GetComponent<Light>().color); // light1�� 3�� �������� Ÿ������.
            }
            else if (rayHit2.distance <= detectDistance && rayHit3.distance <= detectDistance)
            {
                veryClose(rayHit2.collider.GetComponent<Light>().color, rayHit3.collider.GetComponent<Light>().color); // light2�� 3�� �������� Ÿ������.
            }
            else if (rayHit1.distance <= detectDistance)
            {
                veryClose(rayHit1.collider.GetComponent<Light>().color); // light1�� �������� Ÿ������.
            }
            else if (rayHit2.distance <= detectDistance)
            {
                veryClose(rayHit2.collider.GetComponent<Light>().color); // light2�� �������� Ÿ������.
            }
            else if (rayHit3.distance <= detectDistance)
            {
                veryClose(rayHit3.collider.GetComponent<Light>().color); // light3�� �������� Ÿ������.
            }
            else // �Ÿ��� �ణ ����� �Һ� ũ�Ⱑ ���� Ŀ����.
            {
                slightlyClose(Mathf.Min(rayHit1.distance, rayHit2.distance, rayHit3.distance));
            }
        }
        // Ÿ�� 1�� 2�� ���� ���̴�.
        else if (ray1 && ray2 && rayHit1.collider.gameObject == targetLight1 && rayHit2.collider.gameObject == targetLight2) 
        {
            if (rayHit1.distance <= detectDistance && rayHit2.distance <= detectDistance) 
            {
                veryClose(rayHit1.collider.GetComponent<Light>().color, rayHit2.collider.GetComponent<Light>().color); // light1�� 2�� �������� Ÿ������.
            }
            else if (rayHit1.distance <= detectDistance)
            {
                veryClose(rayHit1.collider.GetComponent<Light>().color); // light1�� �������� Ÿ������.
            }
            else if (rayHit2.distance <= detectDistance)
            {
                veryClose(rayHit2.collider.GetComponent<Light>().color); // light2�� �������� Ÿ������.
            }
            else
            {
                slightlyClose(Mathf.Min(rayHit1.distance, rayHit2.distance));
            }
        }
        // Ÿ�� 1�� 3�� ���� ���̴�.
        else if (ray1 && ray3 && rayHit1.collider.gameObject == targetLight1 && rayHit3.collider.gameObject == targetLight3) 
        {
            if (rayHit1.distance <= detectDistance && rayHit3.distance <= detectDistance) 
            {
                veryClose(rayHit1.collider.GetComponent<Light>().color, rayHit3.collider.GetComponent<Light>().color); // light1�� 3�� �������� Ÿ������.
            }
            else if (rayHit1.distance <= detectDistance)
            {
                veryClose(rayHit1.collider.GetComponent<Light>().color); // light1�� �������� Ÿ������.
            }
            else if (rayHit3.distance <= detectDistance)
            {
                veryClose(rayHit3.collider.GetComponent<Light>().color); // light3�� �������� Ÿ������.
            }
            else
            {
                slightlyClose(Mathf.Min(rayHit1.distance, rayHit3.distance));
            }
        }
        // Ÿ�� 2�� 3�� ���� ���̴�.
        else if (ray2 && ray3 && rayHit2.collider.gameObject == targetLight2 && rayHit3.collider.gameObject == targetLight3) 
        {
            if (rayHit2.distance <= detectDistance && rayHit3.distance <= detectDistance) 
            {
                veryClose(rayHit2.collider.GetComponent<Light>().color, rayHit3.collider.GetComponent<Light>().color); // light2�� 3�� �������� Ÿ������.
            }
            else if (rayHit2.distance <= detectDistance)
            {
                veryClose(rayHit2.collider.GetComponent<Light>().color); // light2�� �������� Ÿ������.
            }
            else if (rayHit3.distance <= detectDistance)
            {
                veryClose(rayHit3.collider.GetComponent<Light>().color); // light3�� �������� Ÿ������.
            }
            else
            {
                slightlyClose(Mathf.Min(rayHit2.distance, rayHit3.distance));
            }
        }
        // Ÿ�� 1�� ���� ���̴�.
        else if (ray1 && rayHit1.collider.gameObject == targetLight1) 
        {
            if (rayHit1.distance <= detectDistance) 
            {
                veryClose(rayHit1.collider.GetComponent<Light>().color); // light1�� �������� Ÿ������.
            }
            else
            {
                slightlyClose(rayHit1.distance);
            }
        }
        // Ÿ�� 2�� ���� ���̴�.
        else if (ray2 && rayHit2.collider.gameObject == targetLight2) 
        {
            if (rayHit2.distance <= detectDistance) 
            {
                veryClose(rayHit2.collider.GetComponent<Light>().color); // light2�� �������� Ÿ������.
            }
            else
            {
                slightlyClose(rayHit2.distance);
            }
        }
        // Ÿ�� 3�� ���� ���̴�.
        else if (ray3 && rayHit3.collider.gameObject == targetLight3)
        {
            if (rayHit3.distance <= detectDistance) 
            {
                veryClose(rayHit3.collider.GetComponent<Light>().color); // light3�� �������� Ÿ������.
            }
            else
            {
                slightlyClose(rayHit3.distance);
            }
        }
        else // �� �� ��~�� �ִ�.
        {
            cannotActivate();
        }
    }

    private void canActivate() // ����ġ�� Ȱ��ȭ�Ѵ�.
    {
        isClose = true;
        isActive = true;
        switchPS.startSize = 2.2f;
        firePS.startColor = originFireColor;
    }

    private void veryClose(Color targetColor) // fire�� �ش� �������� Ȱ��ȭ�ȴ�.
    {
        isClose = true;
        isActive = false;
        switchPS.startSize = 2.2f;
        firePS.startColor = convertColor(targetColor);
    }

    private void veryClose(Color targetColor1, Color targetColor2) // fire�� �� �������� Ȱ��ȭ�ȴ�.
    {
        isClose = true;
        isActive = false;
        switchPS.startSize = 2.2f;
        firePS.startColor = convertColor(targetColor1, targetColor2);
    }
    private void slightlyClose(float targetDistance) // switch�� ũ�Ⱑ ���� Ŀ����.
    {
        isClose = false;
        isActive = false;
        switchPS.startSize = 2.2f - (targetDistance - detectDistance) * 0.4f;
    }

    private void cannotActivate() // switch�� ũ�Ⱑ �ּ��̴�.
    {
        isClose = false;
        isActive = false;
        switchPS.startSize = 0.2f;
    }

    // light�� ���� ���� �̻ڰ� �ٲ㼭 ��ȯ�Ѵ�.
    private Color convertColor(Color lightColor)
    {
        float r = lightColor.r;
        float g = lightColor.g;
        float b = lightColor.b;

        if (r == 1)
        {
            return new Color(1, 0.5f, 0.5f, 0.5f);
        }
        if (g == 1)
        {
            return new Color(0.5f, 1, 0.5f, 0.5f);
        }
        if (b == 1)
        {
            return new Color(0.5f, 0.5f, 1, 0.5f);
        }

        // Debug.Log("convertColor1: no matched color");
        return new Color(r, g, b, 0.5f);
    }

    private Color convertColor(Color lightColor1, Color lightColor2)
    {
        float r1 = lightColor1.r;
        float g1 = lightColor1.g;
        float b1 = lightColor1.b;

        float r2 = lightColor2.r;
        float g2 = lightColor2.g;
        float b2 = lightColor2.b;

        switch (r1, g1, b1, r2, g2, b2)
        {
            case (1, _, _, _, 1, _): // ���� �ʷ�
            case (_, 1, _, 1, _, _):
                return new Color(1, 1, 0.5f, 0.5f);

            case (1, _, _, _, _, 1): // ���� �Ķ�
            case (_, _, 1, 1, _, _):
                return new Color(1, 0.5f, 1, 0.5f);

            case (_, 1, _, _, _, 1): // �ʷ� �Ķ�
            case (_, _, 1, _, 1, _):
                return new Color(0.5f, 1, 1, 0.5f);
        }

        // Debug.Log("convertColor2: no matched color");
        return Color.white;
    }
}
