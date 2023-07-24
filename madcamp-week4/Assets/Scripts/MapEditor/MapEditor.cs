using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapEditor : MonoBehaviour
{
    public static GameObject userSelectedTarget = null; // 사용자가 맵에서 선택한 오브젝트

    public GameObject deleteButtonObject;
    Button deleteButton;

    Ray ray;
    RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        deleteButton = deleteButtonObject.GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        // 마우스 왼쪽 버튼 클릭을 감지
        if (Input.GetMouseButtonDown(0))
        {
            // 마우스 클릭한 화면 좌표를 레이캐스트로 변경하여 3D 월드 좌표로 얻기
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit)) // 레이캐스트가 물체에 부딪힌 경우
            {
                Vector3 clickPosition = hit.point;
                Debug.Log("사용자가 클릭한 3D 월드 좌표: " + clickPosition);

                // 바닥을 클릭했을 때는 무시한다. 그렇지 않은 경우 대상을 가리킨다.
                if (!hit.collider.gameObject.CompareTag("Plane"))
                {
                    userSelectedTarget = hit.collider.gameObject;
                    deleteButton.interactable = true;

                    /*
                    // Cube의 Renderer 컴포넌트 가져오기
                    Renderer renderer = userSelectedTarget.GetComponent<Renderer>();

                    // 기존 머테리얼 복사 (가장자리 색상만 변경하고자 함)
                    Material newMaterial = new Material(renderer.sharedMaterial);

                    // 새로운 머테리얼에 Rim 쉐이더 적용
                    Shader rimShader = Shader.Find("Legacy Shaders/VertexLit");
                    newMaterial.shader = rimShader;

                    // 가장자리 색상 설정
                    newMaterial.SetColor("_RimColor", Color.red);

                    // Cube에 새로운 머테리얼 적용
                    renderer.sharedMaterial = newMaterial;
                    */
                }
            }
        }
    }
}
