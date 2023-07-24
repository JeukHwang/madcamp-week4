using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapEditor : MonoBehaviour
{
    public static GameObject userSelectedTarget = null; // ����ڰ� �ʿ��� ������ ������Ʈ

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
        // ���콺 ���� ��ư Ŭ���� ����
        if (Input.GetMouseButtonDown(0))
        {
            // ���콺 Ŭ���� ȭ�� ��ǥ�� ����ĳ��Ʈ�� �����Ͽ� 3D ���� ��ǥ�� ���
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit)) // ����ĳ��Ʈ�� ��ü�� �ε��� ���
            {
                Vector3 clickPosition = hit.point;
                Debug.Log("����ڰ� Ŭ���� 3D ���� ��ǥ: " + clickPosition);

                // �ٴ��� Ŭ������ ���� �����Ѵ�. �׷��� ���� ��� ����� ����Ų��.
                if (!hit.collider.gameObject.CompareTag("Plane"))
                {
                    userSelectedTarget = hit.collider.gameObject;
                    deleteButton.interactable = true;

                    /*
                    // Cube�� Renderer ������Ʈ ��������
                    Renderer renderer = userSelectedTarget.GetComponent<Renderer>();

                    // ���� ���׸��� ���� (�����ڸ� ���� �����ϰ��� ��)
                    Material newMaterial = new Material(renderer.sharedMaterial);

                    // ���ο� ���׸��� Rim ���̴� ����
                    Shader rimShader = Shader.Find("Legacy Shaders/VertexLit");
                    newMaterial.shader = rimShader;

                    // �����ڸ� ���� ����
                    newMaterial.SetColor("_RimColor", Color.red);

                    // Cube�� ���ο� ���׸��� ����
                    renderer.sharedMaterial = newMaterial;
                    */
                }
            }
        }
    }
}
