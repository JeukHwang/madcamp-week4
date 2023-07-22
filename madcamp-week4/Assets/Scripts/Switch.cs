using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;

public class Switch : MonoBehaviour
{
    public GameObject targetLight1; // 이 스위치가 작동하기 위해 필요한 light이다.
    public GameObject targetLight2 = null;
    public GameObject targetLight3 = null;
    int lightCount; // 필요한 light 개수

    public GameObject targetDoor; // 이 스위치와 연결되어 있는 문이다.
    public float detectDistance = 2f; // 스위치가 활성화되기 위해 요구되는 light와 스위치 사이의 거리

    bool isClose; // 빛과의 거리가 충분히 가까운지 여부
    bool isActive; // 현재 스위치 활성화 여부

    // ray에 맞았을 때의 정보를 담는다.
    RaycastHit rayHit1; 
    RaycastHit rayHit2; 
    RaycastHit rayHit3;

    // ray가 light을 바라보는 방향
    Vector3 rayDirection1;
    Vector3 rayDirection2;
    Vector3 rayDirection3;

    ParticleSystem.MinMaxGradient originFireColor; // 원래의 Fire의 색상

    ParticleSystem.MainModule switchPS; // 해당 스위치의 ParticleSystem의 MainModule
    ParticleSystem.MainModule firePS; // 해당 스위치의 자식인 Fire의 ParticleSystem의 MainModule

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

        // ray의 범위 내로 닿은 첫 번째 오브젝트가 targetLight이다.
        // 만약 ray와 light 사이에 장애물이 있으면 if문이 false가 되어 스위치가 활성화되지 않을 것이다.
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
        

        if (isActive)  // 충분히 가깝고 조건을 만족한다.
        {
            targetDoor.GetComponent<Door>().shouldOpen = true;

            transform.GetChild(0).gameObject.SetActive(true); // Sparks 활성화
            transform.GetChild(1).gameObject.SetActive(true); // Fire 활성화
            transform.GetChild(2).gameObject.SetActive(true); // Smoke 활성화
        }
        else if (isClose) // 조건을 만족하지는 않으나, 충분히 가깝다.
        {
            targetDoor.GetComponent<Door>().shouldOpen= false;

            transform.GetChild(0).gameObject.SetActive(false); // Sparks 비활성화
            transform.GetChild(1).gameObject.SetActive(true); // Fire 활성화 (가까이 온 대상의 색깔로 fire만 활성화된다)
            transform.GetChild(2).gameObject.SetActive(false); // Smoke 비활성화
        }
        else
        {
            targetDoor.GetComponent<Door>().shouldOpen = false;

            transform.GetChild(0).gameObject.SetActive(false); // Sparks 비활성화
            transform.GetChild(1).gameObject.SetActive(false); // Fire 비활성화
            transform.GetChild(2).gameObject.SetActive(false); // Smoke 비활성화
        }
    }

    private void lightCount1() // 타깃 light가 1개이다.
    {
        rayDirection1 = (targetLight1.transform.position - transform.position).normalized;

        // 타깃을 향해 쏜 ray가 부딪혔고, 대상이 타깃이다.
        if (Physics.Raycast(transform.position, rayDirection1, out rayHit1, detectDistance + 5) && rayHit1.collider.gameObject == targetLight1)
        {
            if (rayHit1.distance <= detectDistance) // 거리가 충분히 가까워 스위치가 활성화된다.
            {
                isClose = true;
                isActive = true;
                switchPS.startSize = 2.2f;

            }
            else // 거리가 약간 가까워 불빛 크기가 점점 커진다.
            {
                slightlyClose(rayHit1.distance);
            }
        }
        else // 너~무 멀거나, 부딪힌 대상이 타깃이 아니다.
        {
            cannotActivate();
        }
    }

    private void lightCount2() // 타깃 light가 2개이다.
    {
        rayDirection1 = (targetLight1.transform.position - transform.position).normalized;
        rayDirection2 = (targetLight2.transform.position - transform.position).normalized;

        // 범위 내에서 충돌 여부 확인
        bool ray1 = Physics.Raycast(transform.position, rayDirection1, out rayHit1, detectDistance + 5);
        bool ray2 = Physics.Raycast(transform.position, rayDirection2, out rayHit2, detectDistance + 5);

        // 둘 다 범위 내에서 충돌이 있고, 충돌 대상 모두 타깃이다.
        if (ray1 && ray2 && rayHit1.collider.gameObject == targetLight1 && rayHit2.collider.gameObject == targetLight2) 
        {
            if (rayHit1.distance <= detectDistance && rayHit2.distance <= detectDistance) // 둘 다 거리가 충분히 가까워 스위치가 활성화된다.
            {
                canActivate();

            }
            else if (rayHit1.distance <= detectDistance) 
            {
                veryClose(rayHit1.collider.GetComponent<Light>().color); // light1의 색상으로 타오른다.
            }
            else if (rayHit2.distance <= detectDistance) 
            {
                veryClose(rayHit2.collider.GetComponent<Light>().color); // light2의 색상으로 타오른다.
            }
            else // 거리가 약간 가까워 불빛 크기가 점점 커진다.
            {
                slightlyClose(Mathf.Min(rayHit1.distance, rayHit2.distance));
            }
        }
        else if (ray1 && rayHit1.collider.gameObject == targetLight1) // 타깃 1만 범위 내이다.
        {
            if (rayHit1.distance <= detectDistance) 
            {
                veryClose(rayHit1.collider.GetComponent<Light>().color); // light1의 색상으로 타오른다.
            }
            else 
            {
                slightlyClose(rayHit1.distance);
            }
        }
        else if (ray2 && rayHit2.collider.gameObject == targetLight2) // 타깃 2만 범위 내이다.
        {
            if (rayHit2.distance <= detectDistance) 
            {
                veryClose(rayHit2.collider.GetComponent<Light>().color); // light2의 색상으로 타오른다.
            }
            else
            {
                slightlyClose(rayHit2.distance);
            }
        }
        else // 둘 다 너~무 멀다.
        {
            cannotActivate();
        }
    }

    private void lightCount3() // 타깃 light가 3개이다.
    {
        rayDirection1 = (targetLight1.transform.position - transform.position).normalized;
        rayDirection2 = (targetLight2.transform.position - transform.position).normalized;
        rayDirection3 = (targetLight3.transform.position - transform.position).normalized;

        // 범위 내에서 충돌 여부 확인
        bool ray1 = Physics.Raycast(transform.position, rayDirection1, out rayHit1, detectDistance + 5);
        bool ray2 = Physics.Raycast(transform.position, rayDirection2, out rayHit2, detectDistance + 5);
        bool ray3 = Physics.Raycast(transform.position, rayDirection3, out rayHit3, detectDistance + 5);

        // 셋 다 범위 내에서 충돌이 있고, 충돌 대상 모두 타깃이다.
        if (ray1 && ray2 && ray3 && rayHit1.collider.gameObject == targetLight1 && rayHit2.collider.gameObject == targetLight2 && rayHit3.collider.gameObject == targetLight3)
        {
            if (rayHit1.distance <= detectDistance && rayHit2.distance <= detectDistance && rayHit3.distance <= detectDistance) // 셋 다 거리가 충분히 가까워 스위치가 활성화된다.
            {
                canActivate();

            }
            else if (rayHit1.distance <= detectDistance && rayHit2.distance <= detectDistance)
            {
                veryClose(rayHit1.collider.GetComponent<Light>().color, rayHit2.collider.GetComponent<Light>().color); // light1과 2의 색상으로 타오른다.
            }
            else if (rayHit1.distance <= detectDistance && rayHit3.distance <= detectDistance)
            {
                veryClose(rayHit1.collider.GetComponent<Light>().color, rayHit3.collider.GetComponent<Light>().color); // light1과 3의 색상으로 타오른다.
            }
            else if (rayHit2.distance <= detectDistance && rayHit3.distance <= detectDistance)
            {
                veryClose(rayHit2.collider.GetComponent<Light>().color, rayHit3.collider.GetComponent<Light>().color); // light2과 3의 색상으로 타오른다.
            }
            else if (rayHit1.distance <= detectDistance)
            {
                veryClose(rayHit1.collider.GetComponent<Light>().color); // light1의 색상으로 타오른다.
            }
            else if (rayHit2.distance <= detectDistance)
            {
                veryClose(rayHit2.collider.GetComponent<Light>().color); // light2의 색상으로 타오른다.
            }
            else if (rayHit3.distance <= detectDistance)
            {
                veryClose(rayHit3.collider.GetComponent<Light>().color); // light3의 색상으로 타오른다.
            }
            else // 거리가 약간 가까워 불빛 크기가 점점 커진다.
            {
                slightlyClose(Mathf.Min(rayHit1.distance, rayHit2.distance, rayHit3.distance));
            }
        }
        // 타깃 1과 2만 범위 내이다.
        else if (ray1 && ray2 && rayHit1.collider.gameObject == targetLight1 && rayHit2.collider.gameObject == targetLight2) 
        {
            if (rayHit1.distance <= detectDistance && rayHit2.distance <= detectDistance) 
            {
                veryClose(rayHit1.collider.GetComponent<Light>().color, rayHit2.collider.GetComponent<Light>().color); // light1과 2의 색상으로 타오른다.
            }
            else if (rayHit1.distance <= detectDistance)
            {
                veryClose(rayHit1.collider.GetComponent<Light>().color); // light1의 색상으로 타오른다.
            }
            else if (rayHit2.distance <= detectDistance)
            {
                veryClose(rayHit2.collider.GetComponent<Light>().color); // light2의 색상으로 타오른다.
            }
            else
            {
                slightlyClose(Mathf.Min(rayHit1.distance, rayHit2.distance));
            }
        }
        // 타깃 1과 3만 범위 내이다.
        else if (ray1 && ray3 && rayHit1.collider.gameObject == targetLight1 && rayHit3.collider.gameObject == targetLight3) 
        {
            if (rayHit1.distance <= detectDistance && rayHit3.distance <= detectDistance) 
            {
                veryClose(rayHit1.collider.GetComponent<Light>().color, rayHit3.collider.GetComponent<Light>().color); // light1과 3의 색상으로 타오른다.
            }
            else if (rayHit1.distance <= detectDistance)
            {
                veryClose(rayHit1.collider.GetComponent<Light>().color); // light1의 색상으로 타오른다.
            }
            else if (rayHit3.distance <= detectDistance)
            {
                veryClose(rayHit3.collider.GetComponent<Light>().color); // light3의 색상으로 타오른다.
            }
            else
            {
                slightlyClose(Mathf.Min(rayHit1.distance, rayHit3.distance));
            }
        }
        // 타깃 2와 3만 범위 내이다.
        else if (ray2 && ray3 && rayHit2.collider.gameObject == targetLight2 && rayHit3.collider.gameObject == targetLight3) 
        {
            if (rayHit2.distance <= detectDistance && rayHit3.distance <= detectDistance) 
            {
                veryClose(rayHit2.collider.GetComponent<Light>().color, rayHit3.collider.GetComponent<Light>().color); // light2와 3의 색상으로 타오른다.
            }
            else if (rayHit2.distance <= detectDistance)
            {
                veryClose(rayHit2.collider.GetComponent<Light>().color); // light2의 색상으로 타오른다.
            }
            else if (rayHit3.distance <= detectDistance)
            {
                veryClose(rayHit3.collider.GetComponent<Light>().color); // light3의 색상으로 타오른다.
            }
            else
            {
                slightlyClose(Mathf.Min(rayHit2.distance, rayHit3.distance));
            }
        }
        // 타깃 1만 범위 내이다.
        else if (ray1 && rayHit1.collider.gameObject == targetLight1) 
        {
            if (rayHit1.distance <= detectDistance) 
            {
                veryClose(rayHit1.collider.GetComponent<Light>().color); // light1의 색상으로 타오른다.
            }
            else
            {
                slightlyClose(rayHit1.distance);
            }
        }
        // 타깃 2만 범위 내이다.
        else if (ray2 && rayHit2.collider.gameObject == targetLight2) 
        {
            if (rayHit2.distance <= detectDistance) 
            {
                veryClose(rayHit2.collider.GetComponent<Light>().color); // light2의 색상으로 타오른다.
            }
            else
            {
                slightlyClose(rayHit2.distance);
            }
        }
        // 타깃 3만 범위 내이다.
        else if (ray3 && rayHit3.collider.gameObject == targetLight3)
        {
            if (rayHit3.distance <= detectDistance) 
            {
                veryClose(rayHit3.collider.GetComponent<Light>().color); // light3의 색상으로 타오른다.
            }
            else
            {
                slightlyClose(rayHit3.distance);
            }
        }
        else // 셋 다 너~무 멀다.
        {
            cannotActivate();
        }
    }

    private void canActivate() // 스위치를 활성화한다.
    {
        isClose = true;
        isActive = true;
        switchPS.startSize = 2.2f;
        firePS.startColor = originFireColor;
    }

    private void veryClose(Color targetColor) // fire만 해당 색상으로 활성화된다.
    {
        isClose = true;
        isActive = false;
        switchPS.startSize = 2.2f;
        firePS.startColor = convertColor(targetColor);
    }

    private void veryClose(Color targetColor1, Color targetColor2) // fire만 두 색상으로 활성화된다.
    {
        isClose = true;
        isActive = false;
        switchPS.startSize = 2.2f;
        firePS.startColor = convertColor(targetColor1, targetColor2);
    }
    private void slightlyClose(float targetDistance) // switch의 크기가 점점 커진다.
    {
        isClose = false;
        isActive = false;
        switchPS.startSize = 2.2f - (targetDistance - detectDistance) * 0.4f;
    }

    private void cannotActivate() // switch의 크기가 최소이다.
    {
        isClose = false;
        isActive = false;
        switchPS.startSize = 0.2f;
    }

    // light의 색을 보다 이쁘게 바꿔서 반환한다.
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
            case (1, _, _, _, 1, _): // 빨강 초록
            case (_, 1, _, 1, _, _):
                return new Color(1, 1, 0.5f, 0.5f);

            case (1, _, _, _, _, 1): // 빨강 파랑
            case (_, _, 1, 1, _, _):
                return new Color(1, 0.5f, 1, 0.5f);

            case (_, 1, _, _, _, 1): // 초록 파랑
            case (_, _, 1, _, 1, _):
                return new Color(0.5f, 1, 1, 0.5f);
        }

        // Debug.Log("convertColor2: no matched color");
        return Color.white;
    }
}
